namespace Axh.PageTracker.Application.Handlers
{
    using Xilium.CefGlue;

    internal sealed class WebLifeSpanHandler : CefLifeSpanHandler
    {
        private readonly ScreenshotCore core;

        public WebLifeSpanHandler(ScreenshotCore core)
        {
            this.core = core;
        }

        protected override void OnAfterCreated(CefBrowser browser)
        {
            this.core.OnCreated(browser);
        }
        
    }
}
