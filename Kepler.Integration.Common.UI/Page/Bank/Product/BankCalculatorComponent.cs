using System;
using System.Linq;
using System.Threading;
using Kepler.Integration.Common.UI.Page.Common;
using Kepler.Integration.Common.UI.SelenoExtension;
using OpenQA.Selenium;

namespace Kepler.Integration.Common.UI.Page.Bank.Product
{
    public class BankCalculatorComponent : CommonComponent
    {
        public int GetAmount()
        {
            return Convert.ToInt32(GetAmountFieldElement().GetAttribute("value").Replace(" ", ""));
        }

        public IWebElement GetAmountFieldElement()
        {
            return Find.Element(By.CssSelector("div.slider-control > input[name='amount']"));
        }

        public void FillFilterAmountField(int amount)
        {
            var amountField = GetAmountFieldElement();
            amountField.TypeText(amount.ToString());

            //Нужно что бы введенная сумма "зафиксировалась" часть багов падают именно на том что сумма введена а фильтр не применился 
            Thread.Sleep(500);
            ClickInFreeSpace();
        }

        public int GetInitialAmount()
        {
            return Convert.ToInt32(GetInitialAmountFieldElement().GetAttribute("value").Replace(" ", ""));
        }

        public IWebElement GetInitialAmountFieldElement()
        {
            return Find.Element(By.CssSelector("div.slider-control > input[name='initialAmount']"));
        }

        public void FillFilterInitialAmountField(int initialAmount)
        {
            var amountField = GetInitialAmountFieldElement();
            amountField.TypeText(initialAmount.ToString());

            ClickInFreeSpace();
        }

        public string GetSelectedPeriodName()
        {
            return GetSelectedPeriodElement().Text.Trim();
        }

        public IWebElement GetSelectedPeriodElement()
        {
            var periodField = Find.Elements(By.CssSelector("div.calculator-controls-container div.select-control"))
                .Where(element => !element.GetAttribute("class").Contains("select-control--currency"))
                .FirstOrDefault(element => element.FindElement(By.CssSelector("div > label")).Text.Contains("Срок"));

            return periodField.FindElement(By.CssSelector("div.select-control-inner-link > div"));
        }

        public virtual void SelectFilterPeriod(string periodName)
        {
            // expand menu - click on  currently selected period in list (main control)
            GetSelectedPeriodElement().Click();

            var periodElement = GetDriver()
                .WaitAllElementsAndReturnExpected(
                    By.CssSelector("div.select-control-inner-dd li.ik_select_option > span"),
                    element => element.Displayed && element.Text == periodName);

            periodElement.Click();
        }

        public void SelectFilterCurrency(string depositCurrencySymbol)
        {
            // currently selected currency in list (main control)
            Find.Element(
                By.CssSelector(
                    "div.select-control-inner--currency div.select-control-inner--currency-link > div.ik_select_link_text"))
                .Click();

            var periodElement = GetDriver()
                .WaitAllElementsAndReturnExpected(
                    By.CssSelector("div.select-control-inner--currency-dd li.ik_select_option > span"),
                    element => element.Displayed && element.Text == depositCurrencySymbol);

            periodElement.Click();
        }

        public virtual BankAdditionalFilterComponent AdditionalFilters
        {
            get { return GetComponent<BankAdditionalFilterComponent>(); }
        }
    }
}