namespace Axh.PageTracker.DependencyInjection
{
    using Axh.Core.Services.Logging;
    using Axh.Core.Services.Logging.Contracts;
    using Axh.PageTracker.Application;
    using Axh.PageTracker.Application.Config;
    using Axh.PageTracker.Application.Contracts;

    using Ninject.Modules;

    public class SubProcessApplicationModule : NinjectModule
    {
        public override void Load()
        {
            this.Kernel.Bind<ISubProcessService>().To<SubProcessService>();
            this.Kernel.Bind<ICefConfig>().To<SubProcessCefConfig>();

            // Use console logger as don't have a app.config available when sub-process is copied as a library to web project.
            this.Kernel.Bind<ILoggingService>().To<ConsoleLoggingService>();
        }
    }
}
