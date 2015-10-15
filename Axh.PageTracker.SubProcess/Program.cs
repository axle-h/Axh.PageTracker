namespace Axh.PageTracker.SubProcess
{
    using System;
    using System.IO;

    using Axh.PageTracker.Application.Contracts;
    using Axh.PageTracker.DependencyInjection;

    using Ninject;

    class Program
    {
        private static int Main(string[] args)
        {
            // Running this from directory with WebApi extension. Need to ensure we don't try to load it as we don't have a dependency on System.Web.
            var settings = new NinjectSettings { LoadExtensions = false };
            using (var kernel = new StandardKernel(settings, new SubProcessApplicationModule()))
            {
                var subProcess = kernel.Get<ISubProcessService>();
                var returnCode = subProcess.Initialize(args);
                return returnCode;
            }
        }
    }
}
