using System.Collections.ObjectModel;
using Kepler.Integration.Common.UI.Page.Common;
using OpenQA.Selenium;

namespace Kepler.Integration.Common.UI.Page.Bank.Product.ProductDetailsPage
{
    public abstract class BaseBlockAtDetailsPageComponent : CommonPage
    {
        public abstract IWebElement GetCurrentBlockElement();

        public ReadOnlyCollection<IWebElement> GetTextElementsFromConditionsBlock()
        {
            return GetCurrentBlockElement().FindElements(By.CssSelector("span.text"));
        }
    }
}