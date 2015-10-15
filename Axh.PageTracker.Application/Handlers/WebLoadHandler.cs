namespace Axh.PageTracker.Application.Handlers
{
    using Xilium.CefGlue;

    internal sealed class WebLoadHandler : CefLoadHandler
    {
        private readonly ScreenshotCore core;

        public WebLoadHandler(ScreenshotCore core)
        {
            this.core = core;
        }
        
        protected override void OnLoadingStateChange(CefBrowser browser, bool isLoading, bool canGoBack, bool canGoForward)
        {
            this.core.OnLoadingStateChanged(isLoading);
        }
        
        protected override void OnLoadError(CefBrowser browser, CefFrame frame, CefErrorCode errorCode, string errorText, string failedUrl)
        {
            this.core.OnLoadError(errorCode);
        }
    }
}
