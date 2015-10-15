namespace Axh.PageTracker.Application
{
    using System;

    using Axh.Core.Services.Logging.Contracts;
    using Axh.PageTracker.Application.Handlers;

    using Xilium.CefGlue;

    internal sealed class ScreenshotCefClient : CefClient
    {
        private readonly WebLifeSpanHandler lifeSpanHandler;
        private readonly WebLoadHandler loadHandler;
        private readonly ScreenshotRenderHandler renderHandler;
        private readonly ILoggingService loggingService;

        public ScreenshotCefClient(ScreenshotCore core, int width, int height, ILoggingService loggingService)
        {
            this.lifeSpanHandler = new WebLifeSpanHandler(core);
            this.loadHandler = new WebLoadHandler(core);
            this.renderHandler = new ScreenshotRenderHandler(core, width, height);
            this.loggingService = loggingService;
        }

        protected override CefLifeSpanHandler GetLifeSpanHandler()
        {
            return this.lifeSpanHandler;
        }
        
        protected override CefLoadHandler GetLoadHandler()
        {
            return this.loadHandler;
        }

        protected override CefRenderHandler GetRenderHandler()
        {
            return this.renderHandler;
        }

        protected override bool OnProcessMessageReceived(CefBrowser browser, CefProcessId sourceProcess, CefProcessMessage message)
        {
            if (!this.loggingService.IsDebugEnabled)
            {
                return false;
            }

            this.loggingService.Debug("Client::OnProcessMessageReceived: SourceProcess={0}", sourceProcess);
            this.loggingService.Debug("Message Name={0} IsValid={1} IsReadOnly={2}", message.Name, message.IsValid, message.IsReadOnly);
            var arguments = message.Arguments;
            for (var i = 0; i < arguments.Count; i++)
            {
                var type = arguments.GetValueType(i);
                object value;
                switch (type)
                {
                    case CefValueType.Null: value = null; break;
                    case CefValueType.String: value = arguments.GetString(i); break;
                    case CefValueType.Int: value = arguments.GetInt(i); break;
                    case CefValueType.Double: value = arguments.GetDouble(i); break;
                    case CefValueType.Bool: value = arguments.GetBool(i); break;
                    default: value = null; break;
                }

                this.loggingService.Debug("  [{0}] ({1}) = {2}", i, type, value);
            }

            return false;
        }
    }
}
