using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using OpenQA.Selenium;

namespace Kepler.Integration.Common.UI.SelenoExtension
{
    public static class WebDriverExtension
    {
        public static void SwitchToWindow(this IWebDriver webDriver, Expression<Func<IWebDriver, bool>> predicateExp,
            TimeSpan? timeout = null)
        {
            var predicate = predicateExp.Compile();

            if (!timeout.HasValue)
                timeout = TimeSpan.FromSeconds(90);

            var finishTime = DateTime.Now + timeout.Value;

            do
            {
                Thread.Sleep(300);

                foreach (var handle in webDriver.WindowHandles)
                {
                    webDriver.SwitchTo().Window(handle);
                    Console.WriteLine("Tab title: " + webDriver.Title);

                    if (predicate(webDriver))
                    {
                        return;
                    }
                }
            } while (DateTime.Now < finishTime);

            throw new ArgumentException(String.Format("Unable to find window with condition: '{0}'", predicateExp.Body));
        }


        public static void SetAttribute(this IWebDriver driver, string jquerySelector, string attributeName,
            string value)
        {
            var js = (IJavaScriptExecutor) driver;
            js.ExecuteScript(String.Format("$({0}).attr(arguments[0], arguments[1]);", jquerySelector), attributeName,
                value);

            Thread.Sleep(1000);
        }


        public static void SwitchToNewTab(this IWebDriver driver)
        {
            var timeout = TimeSpan.FromSeconds(20);

            var finishTime = DateTime.Now + timeout;

            do
            {
                Thread.Sleep(300);

                if (driver.WindowHandles.Count > 1)
                {
                    driver.SwitchTo().Window(driver.WindowHandles.LastOrDefault());
                    return;
                }
            } while (DateTime.Now < finishTime);

            throw new TimeoutException("Cannot find new tab");
        }


        public static void CloseAnotherTabs(this IWebDriver driver)
        {
            while (driver.WindowHandles.Count > 1)
            {
                driver.SwitchTo().Window(driver.WindowHandles.LastOrDefault());
                driver.Close();
            }

            driver.SwitchTo().Window(driver.WindowHandles.LastOrDefault());
        }
    }
}