using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace Kepler.Integration.Common.UI.SelenoExtension
{
    public static class WebDriverExtension
    {
        public static IWebElement WaitAndFindElement(this IWebDriver webDriver, By by, TimeSpan? timeout = null)
        {
            if (!timeout.HasValue)
                timeout = TimeSpan.FromSeconds(20);

            webDriver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(3));

            try
            {
                WaitForReady(webDriver, timeout.Value);
                WebDriverWait wait = new WebDriverWait(webDriver, timeout.Value);
                return wait.Until(driver =>
                    (driver.FindElements(@by).Count > 0
                     && driver.FindElement(@by).Displayed
                     && driver.FindElement(@by).Enabled)
                        ? driver.FindElement(@by)
                        : null);
            }
            catch
            {
                return null;
            }
            finally
            {
                webDriver.Manage().Timeouts().ImplicitlyWait(Config.Timeout);
            }
        }


        public static bool WaitAllElementsMatchExpression(this IWebDriver webDriver, By elementsSelector,
            Func<IWebElement, bool> predicateExp, TimeSpan? timeout = null)
        {
            if (!timeout.HasValue)
                timeout = TimeSpan.FromSeconds(20);

            var finishTime = DateTime.Now + timeout.Value;

            do
            {
                Thread.Sleep(300);

                try
                {
                    var items = webDriver.FindElements(elementsSelector);

                    var operationResult = items.All(predicateExp);

                    if (operationResult)
                    {
                        return true;
                    }
                }
                catch (StaleElementReferenceException)
                {
                }
            } while (DateTime.Now < finishTime);

            throw new TimeoutException(
                string.Format("Cannot find elements, matched expression. Timeout in {0} sec is exceeded",
                    timeout.Value.TotalSeconds));
        }


        public static IWebElement WaitAllElementsAndReturnExpected(this IWebDriver webDriver, By elementsSelector,
            Func<IWebElement, bool> predicateExp, TimeSpan? timeout = null)
        {
            if (!timeout.HasValue)
                timeout = TimeSpan.FromSeconds(20);

            var finishTime = DateTime.Now + timeout.Value;

            do
            {
                Thread.Sleep(300);

                var items = webDriver.FindElements(elementsSelector);
                var expectedElement = items.FirstOrDefault(predicateExp);

                if (expectedElement != null)
                {
                    return expectedElement;
                }
            } while (DateTime.Now < finishTime);

            throw new TimeoutException("Cannot find expected element in list of elements");
        }


        public static void WaitUntilElementInvisible(this IWebDriver webDriver, By by, TimeSpan? timeout = null)
        {
            WaitAllElementsAndReturnExpected(webDriver, @by, element => element.Displayed, timeout);
        }


        public static void WaitUntilElementIsVisible(this IWebDriver webDriver, By by, TimeSpan timeout,
            bool isShouldBeDisplayed = false)
        {
            webDriver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(3));

            try
            {
                var finishTime = DateTime.Now + timeout;

                do
                {
                    Console.WriteLine("Waiting until element is visible");
                    Thread.Sleep(300);

                    if (DateTime.Now > finishTime)
                        throw new TimeoutException("Element(s) still displayed, but timeout in " + timeout.TotalSeconds +
                                                   " sec exceeded");

                    if (!isShouldBeDisplayed &&
                        (webDriver.FindElements(@by) == null || webDriver.FindElements(@by).Count == 0))
                    {
                        Console.WriteLine("Element(s) wasn't found. Get out of here :)");
                        break;
                    }
                } while (webDriver.FindElements(@by).All(element => element.Displayed != isShouldBeDisplayed));
            }
            catch (NoSuchElementException ex)
            {
                Console.WriteLine("Element wasn't found. Get out of here :)");
            }
            finally
            {
                webDriver.Manage().Timeouts().ImplicitlyWait(Config.Timeout);
            }
        }

        private static void WaitForReady(this IWebDriver webDriver, TimeSpan secondsToWait)
        {
            var wait = new WebDriverWait(webDriver, secondsToWait);
            wait.Until(driver => (bool) ((IJavaScriptExecutor) driver).
                ExecuteScript("return jQuery.active == 0"));
        }

        public static ReadOnlyCollection<IWebElement> FindElements(this IWebDriver webDriver, By by,
            int timeoutInSeconds)
        {
            try
            {
                if (timeoutInSeconds > 0)
                {
                    var wait = new WebDriverWait(webDriver, TimeSpan.FromSeconds(timeoutInSeconds));
                    return wait.Until(drv => (drv.FindElements(@by).Count > 0) ? drv.FindElements(@by) : null);
                }
                return webDriver.FindElements(@by);
            }
            finally
            {
                webDriver.Manage().Timeouts().ImplicitlyWait(Config.Timeout);
            }
        }


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