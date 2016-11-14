using System.Collections.ObjectModel;
using System.Linq;
using OpenQA.Selenium;

namespace Kepler.Integration.Common.UI.Page.Bank.Product.ProductDetailsPage
{
    public class RateAndPaymentBlockAtDetailsPageComponent : BaseBlockAtDetailsPageComponent
    {
        private string _blockSelector = "#product-page-app > div > div.credit-rates-block";

        public override IWebElement GetCurrentBlockElement()
        {
            return Find.Element(By.CssSelector(_blockSelector));
        }

        public void ClickCreditRatesButton()
        {
            var elem = GetButtonsInBlock().FirstOrDefault(x => x.Text == "Ставки по кредиту");
            Scroll().ScrollToElement(elem);
            elem.Click();
        }

        public void ClickPaymentsGraficButton()
        {
            var elem = GetButtonsInBlock().FirstOrDefault(x => x.Text == "График платежей");
            Scroll().ScrollToElement(elem);
            elem.Click();
        }

        public void ClickPaymentsTableButton()
        {
            var elem = GetButtonsInBlock().FirstOrDefault(x => x.Text == "Таблица выплат");
            Scroll().ScrollToElement(elem);
            elem.Click();
        }


        private ReadOnlyCollection<IWebElement> GetButtonsInBlock()
        {
            return GetCurrentBlockElement().FindElements(By.CssSelector("ul > li > label > span"));
        }


        public IWebElement GetPaymentsTableElement()
        {
            return Find.Element(By.CssSelector("div.table-gradient"));
        }

        public IWebElement GetPaymentsGraficlement()
        {
            return Find.Element(By.CssSelector("div.ui-schedule-horizontal"));
        }

        public IWebElement GetCreditRatesElement()
        {
            return Find.Element(By.CssSelector("div.ui-panel__table-header"));
        }
    }
}