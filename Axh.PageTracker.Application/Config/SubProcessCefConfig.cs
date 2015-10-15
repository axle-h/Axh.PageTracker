namespace Axh.PageTracker.Application.Config
{
    using System;

    using Axh.PageTracker.Application.Contracts;

    public class SubProcessCefConfig : ICefConfig
    {
        public int ScreenshotWidth
        {
            get
            {
                throw new NotSupportedException();
            }
        }

        public int ScreenshotHeight
        {
            get
            {
                throw new NotSupportedException();
            }
        }

        public TimeSpan PageLoadTimeout
        {
            get
            {
                throw new NotSupportedException();
            }
        }

        public TimeSpan XhrLoadTimeout
        {
            get
            {
                throw new NotSupportedException();
            }
        }

        public string ScreenshotDirectory
        {
            get
            {
                throw new NotSupportedException();
            }
        }

        public string ScreenshotFormat
        {
            get
            {
                throw new NotSupportedException();
            }
        }

        public TimeSpan MinimumLoadingFrameRate
        {
            get
            {
                throw new NotSupportedException();
            }
        }
    }
}
