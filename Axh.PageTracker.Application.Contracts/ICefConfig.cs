namespace Axh.PageTracker.Application.Contracts
{
    using System;

    public interface ICefConfig
    {
        int ScreenshotWidth { get; }

        int ScreenshotHeight { get; }

        TimeSpan PageLoadTimeout { get; }

        TimeSpan XhrLoadTimeout { get; }

        string ScreenshotDirectory { get; }

        string ScreenshotFormat { get; }

        TimeSpan MinimumLoadingFrameRate { get; }
    }
}