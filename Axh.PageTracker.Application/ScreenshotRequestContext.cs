namespace Axh.PageTracker.Application
{
    using System.Threading.Tasks;

    public class ScreenshotRequestContext
    {
        public ScreenshotRequestContext(string url)
        {
            this.Url = url;
            this.TaskCompletionSource = new TaskCompletionSource<string>();
        }

        public string Url { get; }

        public TaskCompletionSource<string> TaskCompletionSource { get; }

        public void SetBadUrl()
        {
            this.TaskCompletionSource.SetResult(null);
        }
    }
}
