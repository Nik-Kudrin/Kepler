using System.Linq;
using Kepler.Integration.Common.UI.Page.Bank.Product;
using Kepler.Integration.Common.UI.Page.Bank.Product.ProductDetailsPage;
using OpenQA.Selenium;

namespace Kepler.Integration.Common.UI.Page.Bank.Deposit
{
    public class DepositDetailsPage : BankPropositionDetailsPage
    {
        public DepositRatesBlockAtDetailsPageComponent GetRatesAndPaymentsBlock
        {
            get { return GetComponent<DepositRatesBlockAtDetailsPageComponent>(); }
        }

        public void ClickCalcButtonByName(string buttonName)
        {
            var elements =
                Find.Elements(
                    By.CssSelector("div.radio-control.calculator__radio-control > div > ul > li > label > span"));
            var elem = elements.FirstOrDefault(x => x.Text == buttonName);

            elem.Click();
            WaitUntilPageIsLoaded(); //Ожидание обновлекния диаграммы
            WaitUntilOverlay();
        }

        public void FillMonthlyReplenishmentField(int amount)
        {
            var amountField = GetMonthlyReplenishmentFieldElement();
            amountField.Clear();
            amountField.SendKeys(amount.ToString());
            GetProductNameElement().Click(); //Для того что бы "применилось" введеное число

            //Ожидаине обновления диаграммы.
            WaitUntilPageIsLoaded();
            WaitUntilOverlay();
        }
    }
}