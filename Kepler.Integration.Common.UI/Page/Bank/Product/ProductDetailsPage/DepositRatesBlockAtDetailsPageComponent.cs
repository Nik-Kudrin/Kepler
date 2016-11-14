using System.Collections.ObjectModel;
using System.Linq;
using OpenQA.Selenium;

namespace Kepler.Integration.Common.UI.Page.Bank.Product.ProductDetailsPage
{
    public class DepositRatesBlockAtDetailsPageComponent : BaseBlockAtDetailsPageComponent
    {
        private string _blockSelector = "#product-page-app > div > div.ui-panel:nth-child(4)";

        public override IWebElement GetCurrentBlockElement()
        {
            return Find.Element(By.CssSelector(_blockSelector));
        }

        public void ClickRublesButton()
        {
            var elem = GetButtonsInBlock().FirstOrDefault(x => x.Text == "Рубли");
            Scroll().ScrollToElement(elem);
            elem.Click();
        }

        public void ClickDollarsButton()
        {
            var elem = GetButtonsInBlock().FirstOrDefault(x => x.Text == "Доллары США");
            Scroll().ScrollToElement(elem);
            elem.Click();
        }

        public void ClickEuroButton()
        {
            var elem = GetButtonsInBlock().FirstOrDefault(x => x.Text == "Евро");
            Scroll().ScrollToElement(elem);
            elem.Click();
        }


        private ReadOnlyCollection<IWebElement> GetButtonsInBlock()
        {
            return GetCurrentBlockElement().FindElements(By.CssSelector("ul > li > label > span"));
        }


        public IWebElement GetCurrencyTableElement()
        {
            return Find.Element(By.CssSelector("table > tbody > tr:nth-child(2) > td:nth-child(1) > span"));
        }
    }
}