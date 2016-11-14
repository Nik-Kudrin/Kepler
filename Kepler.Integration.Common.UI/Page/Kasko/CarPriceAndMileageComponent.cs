using Kepler.Integration.Common.UI.Page.Common;
using OpenQA.Selenium;

namespace Kepler.Integration.Common.UI.Page.Kasko
{
    public class CarPriceAndMileageComponent : CommonComponent
    {
        private const string ComponentSelector =
            "div.serp-calculator-inner-container > div.calculator-controls-container.l-row > div";

        public void ClickConfirmButton()
        {
            var priceComponent = Find.Element(By.CssSelector(ComponentSelector));
            var buttonSelector = priceComponent.FindElement(By.CssSelector("div.btn-control.approve-btn > button"));
            buttonSelector.Click();
        }

        public void EnterCarPrice(string text)
        {
        }
    }
}