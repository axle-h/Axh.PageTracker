namespace Axh.PageTracker.Application
{
    using System;
    using System.Threading.Tasks;

    using Axh.Core.Services.Logging.Contracts;
    using Axh.PageTracker.Application.Contracts;
    using Axh.PageTracker.Application.Contracts.Request;
    using Axh.PageTracker.Application.Contracts.Response;

    public class PageTrackerService : IPageTrackerService
    {
        private readonly PageTrackerCefApp cef;

        private readonly ILoggingService loggingService;

        public PageTrackerService(ICefConfig cefConfig, ILoggingService loggingService)
        {
            this.loggingService = loggingService;
            this.cef = new PageTrackerCefApp(cefConfig, loggingService);
            this.cef.Initialize(false);
        }

        public async Task<TakeScreenshotResponse> TakeScreenShot(TakeScreenshotRequest request)
        {
            if (string.IsNullOrEmpty(request?.Url))
            {
                throw new ArgumentNullException(nameof(request));
            }

            // Fix the url
            var uriString = request.Url;
            Uri uri;
            if (!Uri.TryCreate(uriString, UriKind.Absolute, out uri))
            {
                throw new Exception("Bad url");
            }

            var filename = await this.cef.Browser.TakeScreenshot(uri.ToString());
            return new TakeScreenshotResponse { FileName = filename, Success = !string.IsNullOrEmpty(filename) };
        }

        public void Dispose()
        {
            this.cef.Dispose();
        }


    }
}
