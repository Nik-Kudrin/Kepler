using System;
using System.Threading;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Interactions.Internal;

namespace Kepler.Integration.Common.UI.SelenoExtension
{
    public class ScrollComponent
    {
        private IWebDriver _driver;

        public ScrollComponent(IWebDriver driver)
        {
            _driver = driver;
        }

        public void ScrollToElement(IWebElement element)
        {
            ScrollElementToTopViewPort(element);
        }

        public void ScrollToElement(By selector)
        {
            var element = _driver.FindElement(selector);
            ScrollElementToTopViewPort(element);
        }

        private void ScrollElementToTopViewPort(IWebElement element)
        {
            var actions = new Actions(_driver);
            actions.MoveToElement(element);
            actions.Perform();

            ICoordinates coordinates = ((ILocatable) element).Coordinates;

            var verticalOffsetForDynamicHeaderPopUp = 0; // in case of 'toolbars' in head of page
            var yCoordinate = coordinates.LocationInViewport.Y - verticalOffsetForDynamicHeaderPopUp;

            ((IJavaScriptExecutor) _driver).ExecuteScript(String.Format("window.scrollBy({0}, {1})", 0, yCoordinate));

            Thread.Sleep(1000);
        }
    }
}