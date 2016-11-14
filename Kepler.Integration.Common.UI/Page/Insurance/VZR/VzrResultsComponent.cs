using Kepler.Integration.Common.UI.Page.Common;
using OpenQA.Selenium;

namespace Kepler.Integration.Common.UI.Page.Insurance.VZR
{
    public class VzrResultsComponent : CommonComponent
    {
        public IWebElement Element { get; set; }

        public VzrResultsComponent()
        {
        }

        public string GetItemTitle()
        {
            return Element.FindElement(By.CssSelector("div.results__item__company__logo a"))
                .GetAttribute("Title");
        }

        public VzrOrderPage ClickBuyPolicy()
        {
            var selector = By.CssSelector("div.results__item__buttons a");
            var element = Element.FindElement(selector);
            Scroll().ScrollToElement(element);

            element.Click();

            return GetComponent<VzrOrderPage>();
        }
    }
}