namespace Axh.PageTracker.Application
{
    using System;
    using System.Collections.Concurrent;
    using System.Drawing;
    using System.Drawing.Imaging;
    using System.IO;
    using System.Runtime.InteropServices;
    using System.Threading;
    using System.Threading.Tasks;

    using Axh.Core.Services.Logging.Contracts;
    using Axh.PageTracker.Application.Contracts;

    using Xilium.CefGlue;

    internal sealed class ScreenshotCore : IDisposable
    {
        private const int IsLoadingPollFrequency = 100;
        private const int ScreenshotQueuePollFrequency = 1000;

        private readonly ICefConfig cefConfig;

        private readonly ScreenshotCefClient client;

        private readonly BlockingCollection<ScreenshotRequestContext> screenshotQueue;

        private readonly CancellationTokenSource screenshotWorkerCancellationSource;

        private readonly ILoggingService loggingService;

        private readonly TaskCompletionSource<bool> initializedCompletionSource;

        private TaskCompletionSource<bool> pageLoadCompletionSource;

        private bool isDisposed;

        // The buffer will be so big it's highly likely it will be on the large object heap.
        // It makes sense to pin it and use native double buffering.
        private readonly byte[] buffer;
        private GCHandle bufferGcHandle;

        private DateTime lastPaintTimeStamp;

        public ScreenshotCore(ICefConfig cefConfig, ILoggingService loggingService)
        {
            this.cefConfig = cefConfig;
            this.loggingService = loggingService;
            this.lastPaintTimeStamp = DateTime.UtcNow;
            this.isDisposed = false;
            this.client = new ScreenshotCefClient(this, cefConfig.ScreenshotWidth, cefConfig.ScreenshotHeight, loggingService);
            this.initializedCompletionSource = new TaskCompletionSource<bool>();
            this.screenshotQueue = new BlockingCollection<ScreenshotRequestContext>();

            this.buffer = new byte[cefConfig.ScreenshotHeight * cefConfig.ScreenshotWidth * 4];
            bufferGcHandle = GCHandle.Alloc(buffer, GCHandleType.Pinned);

            this.screenshotWorkerCancellationSource = new CancellationTokenSource();
            Task.Factory.StartNew(() => ScreenshotWorker(this.screenshotWorkerCancellationSource.Token), TaskCreationOptions.LongRunning);
        }
        
        public CefBrowser Browser { get; private set; }

        public async Task Create(CefWindowInfo windowInfo)
        {
            var settings = new CefBrowserSettings { WindowlessFrameRate = 5 };
            CefBrowserHost.CreateBrowser(windowInfo, this.client, settings, string.Empty);
            await this.initializedCompletionSource.Task.ConfigureAwait(false);
        }

        public void OnCreated(CefBrowser browser)
        {
            this.loggingService.Debug("[OnCreated]");
            this.Browser = browser;
            this.initializedCompletionSource.TrySetResult(true);
        }
        
        public void OnLoadingStateChanged(bool isLoading)
        {
            this.loggingService.Debug("[OnLoadingStateChanged] isLoading: " + isLoading);
            if (isLoading || pageLoadCompletionSource == null)
            {
                return;
            }
            
            pageLoadCompletionSource.TrySetResult(true);
        }
        
        public void OnLoadError(CefErrorCode errorCode)
        {
            this.loggingService.Debug("[OnLoadError] errorCode {0}", errorCode);

            pageLoadCompletionSource?.TrySetResult(false);
        }

        /// <summary>
        /// Paint the buffer
        /// All this is actualy doing is copying the unmanaged bytes into a pinned managed array. It's sort of double buffering.
        /// </summary>
        /// <param name="pointer">Pointer to CEF ARGB 32bpp buffer</param>
        public void OnPaint(IntPtr pointer)
        {
            this.loggingService.Debug("[OnPaint]");
            lastPaintTimeStamp = DateTime.UtcNow;

            try
            {
                // Copy buffer into managed memory
                Marshal.Copy(pointer, buffer, 0, buffer.Length);
            }
            catch (Exception e)
            {
                this.loggingService.Error(e, "Failed to copy CEF buffer into managed memory");
            }
        }

        public async Task<string> TakeScreenshot(string url)
        {
            this.loggingService.Debug("[TakeScreenshot] url: " + url);

            var context = new ScreenshotRequestContext(url);
            this.screenshotQueue.Add(context);
            
            return await context.TaskCompletionSource.Task.ConfigureAwait(false);
        }
        
        public void Dispose()
        {
            if (this.isDisposed || this.Browser == null)
            {
                return;
            }

            // This is run as a singleton so not fussed about thread safe disposing
            this.loggingService.Debug("[Dispose]");

            this.isDisposed = true;

            this.screenshotWorkerCancellationSource.Cancel();
            this.screenshotQueue.CompleteAdding();
            this.screenshotQueue.Dispose();

            var host = this.Browser.GetHost();
            host.CloseBrowser(true);
            host.Dispose();

            // Free up the unmanaged pointer into the buffer
            bufferGcHandle.Free();

            this.Browser.Dispose();
        }
        
        private async Task ScreenshotWorker(CancellationToken cancellation)
        {
            // Simple queue based worker
            while (!isDisposed && !cancellation.IsCancellationRequested)
            {
                if (Browser == null)
                {
                    // Guard against empying the queue before init has been called
                    await Task.Delay(ScreenshotQueuePollFrequency, cancellation);
                    continue;
                }

                ScreenshotRequestContext current;
                if (!this.screenshotQueue.TryTake(out current, ScreenshotQueuePollFrequency, cancellation))
                {
                    continue;
                }
                
                this.loggingService.Debug("[ScreenshotWorker] Url: " + current.Url);

                this.pageLoadCompletionSource = new TaskCompletionSource<bool>();
                
                // Set the url and wait for the page to fully load.
                var mainFrame = this.Browser.GetMainFrame();
                mainFrame.LoadUrl(current.Url);

                if (!await RunWithTimeout(() => this.pageLoadCompletionSource.Task, this.cefConfig.PageLoadTimeout, cancellation) || !await this.pageLoadCompletionSource.Task)
                {
                    this.loggingService.Warn("[ScreenshotWorker] Failed to load");
                    pageLoadCompletionSource.TrySetCanceled();
                    current.SetBadUrl();
                    continue;
                }

                // Wait for XHR requests to settle
                // Still try and take a screenshot if this times out
                await RunWithTimeout(
                    async () =>
                    {
                        while (DateTime.UtcNow - lastPaintTimeStamp < this.cefConfig.MinimumLoadingFrameRate)
                        {
                            await Task.Delay(IsLoadingPollFrequency, cancellation);
                        }
                    },
                    this.cefConfig.PageLoadTimeout,
                    cancellation);

                var path = SaveFrame();
                this.Browser.StopLoad();
                current.TaskCompletionSource.SetResult(path);
            }
        }

        /// <summary>
        /// Do the heavy lifting 'ARGB formatted CEF buffer -> PNG -> disc'
        /// </summary>
        /// <returns></returns>
        private string SaveFrame()
        {
            if (buffer == null || buffer.Length == 0)
            {
                return null;
            }

            var fileName = Path.ChangeExtension(Guid.NewGuid().ToString(), this.cefConfig.ScreenshotFormat);
            var path = Path.Combine(this.cefConfig.ScreenshotDirectory, fileName);

            this.loggingService.Debug("[SaveFrame] path: " + path);

            try
            {
                var bufferPointer = bufferGcHandle.AddrOfPinnedObject();
                using (var bitmap = new Bitmap(this.cefConfig.ScreenshotWidth, this.cefConfig.ScreenshotHeight, this.cefConfig.ScreenshotWidth * 4, PixelFormat.Format32bppRgb, bufferPointer))
                using (var file = File.Open(path, FileMode.Create))
                {
                    bitmap.Save(file, ImageFormat.Png);
                }

                return path;
            }
            catch (Exception e)
            {
                this.loggingService.Error(e, "Failed to save file: " + path);
                return null;
            }
        }

        private static async Task<bool> RunWithTimeout(Func<Task> work, TimeSpan timeout, CancellationToken cancellation)
        {
            var workTask = work();
            return await Task.WhenAny(workTask, Task.Delay(timeout, cancellation)) == workTask;
        }
    }
}
