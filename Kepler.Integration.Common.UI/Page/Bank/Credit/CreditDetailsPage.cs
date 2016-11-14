using System.Linq;
using Kepler.Integration.Common.UI.Page.Bank.Product;
using Kepler.Integration.Common.UI.Page.Bank.Product.ProductDetailsPage;
using OpenQA.Selenium;

namespace Kepler.Integration.Common.UI.Page.Bank.Credit
{
    public class CreditDetailsPage : BankPropositionDetailsPage
    {
        public DocumentsBlockAtDetailsPageComponent GetDocumentsBlock
        {
            get { return GetComponent<DocumentsBlockAtDetailsPageComponent>(); }
        }

        public RateAndPaymentBlockAtDetailsPageComponent GetRatesAndPaymentsBlock
        {
            get { return GetComponent<RateAndPaymentBlockAtDetailsPageComponent>(); }
        }

        public void ClickCheckboxByName(string checkboxName)
        {
            var elements = Find.Elements(By.CssSelector("div.calculator__checkbox-control"));
            var elem = elements.FirstOrDefault(x => x.Text == checkboxName);

            elem.Click();
            WaitUntilPageIsLoaded(); //Ожидание обновлекния диаграммы
            WaitUntilOverlay();
        }

        public void ClickCalcButtonByName(string buttonName)
        {
            var elements =
                Find.Elements(
                    By.CssSelector("div.radio-control.calculator__select-control > div > ul > li > label > span"));
            var elem = elements.FirstOrDefault(x => x.Text == buttonName);

            elem.Click();
            WaitUntilPageIsLoaded(); //Ожидание обновлекния диаграммы
            WaitUntilOverlay();
        }
    }
}