using OpenQA.Selenium;
using TestStack.Seleno.PageObjects;

namespace Kepler.Integration.Common.UI.Page.Insurance
{
    public class InsuranceResultComponent : UiComponent
    {
        public IWebElement Element { get; set; }

        public string GetItemTitle()
        {
            return Element.FindElement(By.CssSelector("div.results__item__company__logo a"))
                .GetAttribute("Title");
        }
    }
}