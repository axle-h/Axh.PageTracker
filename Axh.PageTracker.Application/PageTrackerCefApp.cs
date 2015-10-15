namespace Axh.PageTracker.Application
{
    using System;
    using System.IO;

    using Axh.Core.Services.Logging.Contracts;
    using Axh.PageTracker.Application.Contracts;

    using Xilium.CefGlue;

    internal sealed class PageTrackerCefApp : CefApp, IDisposable
    {
        private readonly ILoggingService loggingService;

        private readonly Lazy<ScreenshotCore> browser;
        
        private readonly object disposingContext = new object();
        
        private bool isInitialized;

        private bool isDisposed;
        
        public PageTrackerCefApp(ICefConfig cefConfig, ILoggingService loggingService)
        {
            this.loggingService = loggingService;
            this.isInitialized = false;
            this.isDisposed = false;

            this.browser = new Lazy<ScreenshotCore>(
                () =>
                {
                    var cefWindowInfo = CefWindowInfo.Create();
                    cefWindowInfo.SetAsWindowless(IntPtr.Zero, true);

                    var webBrowser = new ScreenshotCore(cefConfig, loggingService);
                    webBrowser.Create(cefWindowInfo).Wait();
                    
                    return webBrowser;
                },
                false);
        }

        protected override void OnBeforeCommandLineProcessing(string processType, CefCommandLine commandLine)
        {
            commandLine.AppendSwitch("disable-gpu", "1");
            commandLine.AppendSwitch("off-screen-rendering-enabled", "1");
        }

        
        protected override CefBrowserProcessHandler GetBrowserProcessHandler()
        {
            return base.GetBrowserProcessHandler();
        }

        protected override CefRenderProcessHandler GetRenderProcessHandler()
        {
            return base.GetRenderProcessHandler();
        }

        public int Initialize(bool isSubProcess, params string[] args)
        {
            var path = AppDomain.CurrentDomain.BaseDirectory;
            var binPath = isSubProcess ? path : Path.Combine(path, "bin");

            loggingService.Debug("[Initialize] cefPath: {0}", binPath);

            CefRuntime.Load(binPath);

            loggingService.Debug("[Initialize] Cef binaries found");

            var mainArgs = new CefMainArgs(args);

            var exitCode = CefRuntime.ExecuteProcess(mainArgs, this, IntPtr.Zero);
            loggingService.Debug("[Initialize] exitCode: " + exitCode);

            if (exitCode != -1 || isSubProcess)
            {
                // Sub-process will return here
                return exitCode;
            }
            
            var settings = new CefSettings
            {
                SingleProcess = false,
                MultiThreadedMessageLoop = true,
                LogSeverity = CefLogSeverity.Verbose,
                WindowlessRenderingEnabled = true,
                IgnoreCertificateErrors = true,
                PersistSessionCookies = false,
                BrowserSubprocessPath = Path.Combine(binPath, "Axh.PageTracker.SubProcess.exe"),
                LocalesDirPath = Path.Combine(binPath, "locales"),
                Locale = "en-GB",
                LogFile = "C:\\Temp\\Cef.log"
            };

            CefRuntime.Initialize(mainArgs, settings, this, IntPtr.Zero);
            this.isInitialized = true;

            loggingService.Debug("[Initialize] Success");
            return 0;
        }
        
        public ScreenshotCore Browser
        {
            get
            {
                if (!isInitialized || isDisposed)
                {
                    return null;
                }

                return this.browser.Value;
            }
        }

        public void Dispose()
        {
            if (!isInitialized || isDisposed)
            {
                return;
            }

            loggingService.Debug("[Dispose]");

            lock (disposingContext)
            {
                if (isDisposed)
                {
                    return;
                }

                this.isDisposed = true;

                if (this.browser.IsValueCreated)
                {
                    this.browser.Value.Dispose();
                }

                CefRuntime.Shutdown();
            }
        }
    }
}
