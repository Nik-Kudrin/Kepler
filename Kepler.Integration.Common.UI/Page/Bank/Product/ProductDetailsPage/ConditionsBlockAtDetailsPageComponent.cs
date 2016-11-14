using OpenQA.Selenium;

namespace Kepler.Integration.Common.UI.Page.Bank.Product.ProductDetailsPage
{
    public class ConditionsBlockAtDetailsPageComponent : BaseBlockAtDetailsPageComponent
    {
        private string _blockSelector = "#product-page-app > div > div:nth-child(2)";

        public override IWebElement GetCurrentBlockElement()
        {
            return Find.Element(By.CssSelector(_blockSelector));
        }

        public string GetTextMoreButton()
        {
            return GetMoreButtonElement().Text.Trim();
        }

        public void ClickMoreButton()
        {
            var elem = GetMoreButtonElement();
            Scroll().ScrollToElement(elem);
            elem.Click();
        }

        public IWebElement GetMoreButtonElement()
        {
            return GetCurrentBlockElement().FindElement(By.CssSelector("a.ui-panel__more"));
        }
    }
}