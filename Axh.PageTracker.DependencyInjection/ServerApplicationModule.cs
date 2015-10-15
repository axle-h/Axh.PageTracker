namespace Axh.PageTracker.DependencyInjection
{
    using Axh.Core.Services.Logging;
    using Axh.Core.Services.Logging.Contracts;
    using Axh.PageTracker.Application;
    using Axh.PageTracker.Application.Config;
    using Axh.PageTracker.Application.Contracts;

    using Ninject.Modules;

    public class ServerApplicationModule : NinjectModule
    {
        public override void Load()
        {
            this.Kernel.Bind<IPageTrackerService>().To<PageTrackerService>().InSingletonScope();
            this.Kernel.Bind<ICefConfig>().To<ServerCefConfig>().InSingletonScope();
            this.Kernel.Bind<ILoggingService>().To<Log4NetLoggingService>().InSingletonScope().WithConstructorArgument("name", "PageTracker.Server");
        }
    }
}
