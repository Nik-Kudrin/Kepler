using OpenQA.Selenium;

namespace Kepler.Integration.Common.UI.Page.Bank.Product.ProductDetailsPage
{
    public class DocumentsBlockAtDetailsPageComponent : BaseBlockAtDetailsPageComponent
    {
        private readonly string _blockSelector = "#product-page-app > div > div.required-documents";

        public override IWebElement GetCurrentBlockElement()
        {
            return Find.Element(By.CssSelector(_blockSelector));
        }

        public string GetTextMoreButton()
        {
            return GetMoreButtonElement().Text;
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