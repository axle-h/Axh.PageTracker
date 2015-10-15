namespace Axh.PageTracker.Application
{
    using Axh.Core.Services.Logging.Contracts;
    using Axh.PageTracker.Application.Contracts;

    public class SubProcessService : ISubProcessService
    {
        private readonly PageTrackerCefApp cef;
        
        public SubProcessService(ICefConfig cefConfig, ILoggingService loggingService)
        {
            this.cef = new PageTrackerCefApp(cefConfig, loggingService);
        }

        public int Initialize(string[] args)
        {
            return this.cef.Initialize(true, args);
        }
        
        public void Dispose()
        {
            this.cef.Dispose();
        }
    }
}
