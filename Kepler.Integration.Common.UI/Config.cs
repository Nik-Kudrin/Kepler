using System;

namespace Kepler.Integration.Common.UI
{
    public class Config
    {
        public static string ScreenPath { get; set; }
        public static string ServerUnderTest { get; set; }
        public static string SeleniumHubUrl { get; set; }
        public static string BranchName { get; set; }
        public static string HostName { get; set; }
        public static string Port { get; set; }
        public static readonly string FlowId = Guid.NewGuid().ToString();
        public static string BrowserName { get; set; }
        public static TimeSpan Timeout = TimeSpan.FromSeconds(15);

        public static int BuildId { get; set; }
        public static bool EnableTestRailIntegration { get; set; }


        public static void SplitServerName()
        {
            var splitHostnameAndPort = ServerUnderTest.Replace("http://", "")
                .Split(new[] {":"}, StringSplitOptions.None);

            HostName = splitHostnameAndPort[0];

            Port = splitHostnameAndPort.Length == 1 ? "80" : splitHostnameAndPort[1];
        }
    }
}