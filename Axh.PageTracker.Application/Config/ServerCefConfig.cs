namespace Axh.PageTracker.Application.Config
{
    using System;
    using System.Configuration;

    using Axh.PageTracker.Application.Contracts;

    public class ServerCefConfig : ICefConfig
    {
        private const string ScreenshotWidthKey = "Cef_ScreenshotWidth";
        private const string ScreenshotHeightKey = "Cef_ScreenshotHeight";
        private const string PageLoadTimeoutKey = "Cef_PageLoadTimeout_Milliseconds";
        private const string XhrLoadTimeoutKey = "Cef_XhrLoadTimeout_Milliseconds";
        private const string MinimumLoadingFrameRateKey = "Cef_MinimumLoadingFrameRate_Milliseconds";
        private const string ScreenshotDirectoryKey = "Cef_ScreenshotDirectory";
        private const string ScreenshotFormatKey = "Cef_ScreenshotFormat";

        public ServerCefConfig()
        {
            ScreenshotWidth = ParseInt(ScreenshotWidthKey);
            ScreenshotHeight = ParseInt(ScreenshotHeightKey);
            PageLoadTimeout = TimeSpan.FromMilliseconds(ParseInt(PageLoadTimeoutKey));
            XhrLoadTimeout = TimeSpan.FromMilliseconds(ParseInt(XhrLoadTimeoutKey));
            MinimumLoadingFrameRate = TimeSpan.FromMilliseconds(ParseInt(MinimumLoadingFrameRateKey));
            ScreenshotDirectory = ConfigurationManager.AppSettings[ScreenshotDirectoryKey];
            ScreenshotFormat = ConfigurationManager.AppSettings[ScreenshotFormatKey];
        }

        public int ScreenshotWidth { get; }

        public int ScreenshotHeight { get; }

        public TimeSpan PageLoadTimeout { get; }

        public TimeSpan XhrLoadTimeout { get; }

        public string ScreenshotDirectory { get; }

        public string ScreenshotFormat { get; }

        public TimeSpan MinimumLoadingFrameRate { get; }

        private static int ParseInt(string key)
        {
            int result;
            if (int.TryParse(ConfigurationManager.AppSettings[key], out result))
            {
                return result;
            }

            throw new ConfigurationErrorsException(string.Format("Configuration value '{0}' is not a number.", key));
        }
    }
}
