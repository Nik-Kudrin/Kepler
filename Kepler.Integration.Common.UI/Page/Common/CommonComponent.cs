using System;
using System.Threading;
using Kepler.Integration.Common.UI.SelenoExtension;
using OpenQA.Selenium;
using TestStack.Seleno.PageObjects;

namespace Kepler.Integration.Common.UI.Page.Common
{
    public class CommonComponent : UiComponent
    {
        public IWebElement Element { get; set; }

        public ScrollComponent Scroll()
        {
            return new ScrollComponent(Browser);
        }

        public void WaitUntilListingOverlayIsDisplayed(TimeSpan timeout = default(TimeSpan))
        {
            if (timeout == default(TimeSpan))
                timeout = TimeSpan.FromSeconds(10);

            GetDriver().WaitAllElementsMatchExpression(By.CssSelector("div.serp-results > div.results-container"),
                element => !element.GetAttribute("class").Contains("is-inprogress"), timeout);

            Thread.Sleep(4000); // For dom restructuring
        }

        //Метод для обновление выдачи при вводе суммы за пределами допустимой
        public void ClickInFreeSpace()
        {
            Find.Element(By.CssSelector("div.serp-calculator h1")).Click();
        }

        public IWebDriver GetDriver()
        {
            return Browser;
        }
    }
}