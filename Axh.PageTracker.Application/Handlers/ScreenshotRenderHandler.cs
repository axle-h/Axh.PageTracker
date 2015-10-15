namespace Axh.PageTracker.Application.Handlers
{
    using System;

    using Xilium.CefGlue;

    internal sealed class ScreenshotRenderHandler : CefRenderHandler
    {
        private readonly ScreenshotCore core;

        private readonly int screenWidth;
        private readonly int screenHeight;

        public ScreenshotRenderHandler(ScreenshotCore core, int screenWidth, int screenHeight)
        {
            this.core = core;
            this.screenWidth = screenWidth;
            this.screenHeight = screenHeight;
        }

        protected override bool GetRootScreenRect(CefBrowser browser, ref CefRectangle rect)
        {
            rect.X = 0;
            rect.Y = 0;
            rect.Width = this.screenWidth;
            rect.Height = this.screenHeight;

            return true;
        }

        protected override bool GetViewRect(CefBrowser browser, ref CefRectangle rect)
        {
            return this.GetRootScreenRect(browser, ref rect);
        }

        protected override bool GetScreenPoint(CefBrowser browser, Int32 viewX, Int32 viewY, ref Int32 screenX, ref Int32 screenY)
        {
            screenX = viewX;
            screenY = viewY;
            return true;
        }

        protected override bool GetScreenInfo(CefBrowser browser, CefScreenInfo screenInfo)
        {
            var rect = screenInfo.Rectangle;
            this.GetRootScreenRect(browser, ref rect);
            screenInfo.Depth = 32;
            screenInfo.IsMonochrome = false;

            return true;
        }

        protected override void OnPopupSize(CefBrowser browser, CefRectangle rect)
        {
            this.GetRootScreenRect(browser, ref rect);
        }

        protected override void OnPaint(CefBrowser browser, CefPaintElementType type, CefRectangle[] dirtyRects, IntPtr buffer, int width, int height)
        {
            if (type != CefPaintElementType.View)
            {
                // Only want to paint the view
                return;
            }

            this.core.OnPaint(buffer);
        }

        protected override void OnCursorChange(CefBrowser browser, IntPtr cursorHandle, CefCursorType type, CefCursorInfo customCursorInfo)
        {
            
        }

        protected override void OnScrollOffsetChanged(CefBrowser browser)
        {
            
        }
        
    }
}