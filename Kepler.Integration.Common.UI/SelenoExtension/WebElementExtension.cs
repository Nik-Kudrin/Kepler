using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using OpenQA.Selenium;

namespace Kepler.Integration.Common.UI.SelenoExtension
{
    public static class WebElementExtension
    {
        public static IEnumerable<IWebElement> WaitAndFindElements(this IWebElement webElement, By elementsSelector,
            Func<IWebElement, bool> predicateExp, IWebDriver webDriver, TimeSpan? timeout = null)
        {
            if (timeout == null)
                timeout = TimeSpan.FromSeconds(45);
            var finishTime = DateTime.Now + timeout;

            webDriver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(1));

            try
            {
                do
                {
                    Thread.Sleep(300);

                    var items = webElement.FindElements(elementsSelector);
                    var expectedElements = items.Where(predicateExp);

                    if (expectedElements != null)
                    {
                        return expectedElements;
                    }
                } while (DateTime.Now < finishTime);
            }
            catch
            {
                return null;
            }
            finally
            {
                webDriver.Manage().Timeouts().ImplicitlyWait(Config.Timeout);
            }

            throw new TimeoutException(string.Format("Cannot find expected element. Timeout in {0} sec was exceeded", timeout.Value.TotalSeconds));
        }

        /// <summary>
        /// Return null if element isn't found
        /// </summary>
        /// <param name="webElement"></param>
        /// <param name="elementsSelector"></param>
        /// <param name="predicateExp"></param>
        /// <param name="webDriver"></param>
        /// <param name="timeout"></param>
        /// <returns></returns>
        public static IWebElement WaitAndFindElement(this IWebElement webElement, By elementsSelector,
            Func<IWebElement, bool> predicateExp, IWebDriver webDriver, TimeSpan? timeout = null)
        {
            return webElement.WaitAndFindElements(elementsSelector, predicateExp, webDriver, timeout)
                .FirstOrDefault();
        }


        public static void TypeText(this IWebElement element, string text)
        {
            element.Clear();
            element.SendKeys(text);

            // TODO: Maybe we should focus first on element
            // Option 1.
            // element.SendKeys(string.Empty);
            // Option 2.
            // ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].focus();", element);
        }
    }
}