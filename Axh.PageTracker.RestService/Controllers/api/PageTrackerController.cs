namespace Axh.PageTracker.RestService.Controllers.api
{
    using System.Threading.Tasks;
    using System.Web.Http;

    using Axh.PageTracker.Application.Contracts;
    using Axh.PageTracker.Application.Contracts.Request;
    using Axh.PageTracker.Application.Contracts.Response;

    public class PageTrackerController : ApiController
    {
        private readonly IPageTrackerService pageTrackerService;

        public PageTrackerController(IPageTrackerService pageTrackerService)
        {
            this.pageTrackerService = pageTrackerService;
        }

        public async Task<TakeScreenshotResponse> TakeScreenshot(TakeScreenshotRequest request)
        {
            return await this.pageTrackerService.TakeScreenShot(request);
        }
    }
}
