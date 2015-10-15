namespace Axh.PageTracker.Application.Contracts
{
    using System;
    using System.Threading.Tasks;

    using Axh.PageTracker.Application.Contracts.Request;
    using Axh.PageTracker.Application.Contracts.Response;

    public interface IPageTrackerService : IDisposable
    {
        Task<TakeScreenshotResponse> TakeScreenShot(TakeScreenshotRequest request);
    }
}