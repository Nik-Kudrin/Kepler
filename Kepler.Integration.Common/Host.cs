using System;
using Castle.Core.Logging;
using Kepler.Integration.Common.UI;
using OpenQA.Selenium.Remote;
using TestStack.Seleno.Configuration;
using TestStack.Seleno.Configuration.WebServers;

namespace Kepler.Integration.Common
{
    public class Host
    {
        public SelenoHost _host = new SelenoHost();


        public Host()
        {
            InitSelenoHost();
        }

        public void InitSelenoHost()
        {
            if (_host.Application != null)
            {
                _host.Application.WebServer.Stop();
                _host.Dispose();
                _host = new SelenoHost();
            }

            _host.Run(configure => configure
                .WithWebServer(new InternetWebServer(Config.ServerUnderTest))
                .UsingCamera(Config.ScreenPath)
                .UsingLoggerFactory(new ConsoleFactory(LoggerLevel.Info))
                .WithMinimumWaitTimeoutOf(Config.Timeout)
                .WithRemoteWebDriver(GetBrowserFactory(Config.BrowserName, Config.SeleniumHubUrl))
                );

            _host.Application.Browser.Manage().Window.Maximize();
        }


        private static Func<RemoteWebDriver> GetBrowserFactory(string browserName, string seleniumHubUrl)
        {
            DesiredCapabilities capabilities;

            switch (browserName)
            {
                case "ff":
                    capabilities = DesiredCapabilities.Firefox();
                    capabilities.IsJavaScriptEnabled = true;
                    break;
                case "ie_11":
                    capabilities = DesiredCapabilities.InternetExplorer();
                    capabilities.SetCapability("version", 11);
                    break;
                case "chrome":
                    capabilities = DesiredCapabilities.Chrome();
                    break;
                case "chrome_deposit_pixel":
                    capabilities = DesiredCapabilities.Chrome();
                    capabilities.SetCapability("version", "chrome_deposit_pixel");
                    break;
                default:
                    throw new NotSupportedException("Unsupported browser type");
            }

            if (ProxyContainer.Proxy != null)
                capabilities.SetCapability(CapabilityType.Proxy, ProxyContainer.Proxy);

            return
                () => new RemoteWebDriver(new Uri(seleniumHubUrl), capabilities, TimeSpan.FromSeconds(180));
        }
    }
}