using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Kepler.Integration.Common.UI.Page.Bank.Product.ProductDetailsPage;
using Kepler.Integration.Common.UI.Page.Common;
using Kepler.Integration.Common.UI.SelenoExtension;
using OpenQA.Selenium;

namespace Kepler.Integration.Common.UI.Page.Bank.Product
{
    public class BankPropositionDetailsPage : BankPage.BankPage
    {
        public string GetProductName()
        {
            return GetProductNameElement().Text;
        }

        public virtual IWebElement GetProductNameElement()
        {
            return Find.Element(By.CssSelector(".product-inside__top-name"));
        }

        public virtual string GetBankNameFromLogo()
        {
            return Find.Element(By.CssSelector("img.product-inside__top-pic")).GetAttribute("alt");
        }

        public int GetAmount()
        {
            //Данный код &nbsp; видно в дебагерре гугл хрома. Аналогично можно поступать с другими кодами
            var spaceFromHtml = HttpUtility.HtmlDecode("&nbsp;");

            var element = GetAmountFieldElement().GetAttribute("value");

            return Convert.ToInt32(element.Replace(spaceFromHtml, "").Replace(" ", "").Trim());
        }


        public virtual IWebElement ClickGreenLinkGoToSiteAndGetPopUp()
        {
            ClickGreenLinkGoToSiteInCalc();
            return GetComponent<UiPopupComponent>().WaitPopupWindow();
        }


        public void ClickGreenLinkGoToSiteInCalc()
        {
            var elem = Find.Element(By.CssSelector("div.calculator-col--controls__bottom > span"));
            Scroll().ScrollToElement(elem);
            elem.Click();

            WaitUntilPageIsLoaded();
        }

        public void ClickBlueLinkGoToSiteInMiddleOfPAge()
        {
            var elem = Find.Element(By.CssSelector("a.product-inside__go-to-bank"));
            Scroll().ScrollToElement(elem);
            elem.Click();

            WaitUntilPageIsLoaded();
        }

        public IWebElement GetAmountFieldElement()
        {
            return Find.Element(By.CssSelector("div.slider-control > input[name='Amount']"));
        }

        public IWebElement GetAmountSliderElement()
        {
            return
                Find.Element(
                    By.CssSelector("div.slider-control > input[name='Amount'] ~ div.deposit-amount-slider-container"));
        }


        public int GetMonthlyReplenishmentAmount()
        {
            return Convert.ToInt32(GetMonthlyReplenishmentFieldElement().GetAttribute("value").Replace(" ", ""));
        }

        public IWebElement GetMonthlyReplenishmentFieldElement()
        {
            return Find.Element(By.CssSelector("div.slider-control > input[name='RenewalAmount']"));
        }

        public IWebElement GetMonthlyReplenishmentFieldSliderElement()
        {
            return
                Find.Element(
                    By.CssSelector(
                        "div.slider-control > input[name='RenewalAmount'] ~ div.deposit-amount-slider-container"));
        }


        public void FillFilterAmountField(int amount)
        {
            var amountField = GetAmountFieldElement();
            amountField.Clear();
            amountField.SendKeys(amount.ToString());
            GetProductNameElement().Click(); //Для того что бы "применилось" введеное число

            //Ожидаине обновления диаграммы.
            WaitUntilPageIsLoaded();
            WaitUntilOverlay();
        }

        public int GetInitialAmount()
        {
            return Convert.ToInt32(GetInitialAmountFieldElement().GetAttribute("value").Replace(" ", ""));
        }

        public IWebElement GetInitialAmountFieldElement()
        {
            return Find.Element(By.CssSelector("div.slider-control > input[name='InitialAmount']"));
        }

        public IWebElement GetInitialAmountSliderElement()
        {
            return
                Find.Element(
                    By.CssSelector(
                        "div.slider-control > input[name='InitialAmount'] ~ div.deposit-amount-slider-container"));
        }


        public void FillFilterInitialAmountField(int initialAmount)
        {
            var amountField = GetInitialAmountFieldElement();
            amountField.Clear();
            amountField.SendKeys(initialAmount.ToString());
            GetProductNameElement().Click(); //Для того что бы "применилось" введеное число

            //Ожидаине обновления диаграммы.
            WaitUntilPageIsLoaded();
            WaitUntilOverlay();
        }


        public int GetGreyDiagramAmount()
        {
            var element = GetDiagramAmountElement();

            return GetAmountValueFromDiagramElement(element);
        }

        public IWebElement GetDiagramAmountElement()
        {
            return Find.Element(By.CssSelector("li.product-summary__details-item--grey"));
        }


        public int GetDiagramSigmaAmount()
        {
            var element = GetDiagramSigmaAmountElement();

            return GetAmountValueFromDiagramElement(element);
        }

        public IWebElement GetDiagramSigmaAmountElement()
        {
            return Find.Element(By.CssSelector("li.product-summary__details-item--sigma"));
        }


        public int GetDiagramOverpaymentAmount()
        {
            var element = GetDiagramGreenAmountElement();

            return GetAmountValueFromDiagramElement(element);
        }

        public int GetDiagramDepositPercentsAmount()
        {
            var element = GetDiagramGreenAmountElement();

            return GetAmountValueFromDiagramElement(element);
        }

        public string GetDiagramOverpaymentPeriod()
        {
            var element = GetDiagramGreenAmountElement();

            return element.FindElement(By.CssSelector("span.product-summary__clarification > span:nth-child(2)")).Text;
        }

        public string GetDiagramMonthlyReplenishmentPeriod()
        {
            var element = GetDiagramBlueAmountElement();

            return element.FindElement(By.CssSelector("span.product-summary__clarification > span:nth-child(2)")).Text;
        }

        public int GetDiagramReplenishmentForYear()
        {
            var element = GetDiagramBlueAmountElement();

            return GetAmountValueFromDiagramElement(element);
        }

        public IWebElement GetDiagramGreenAmountElement()
        {
            return Find.Element(By.CssSelector("li.product-summary__details-item--green"));
        }

        public IWebElement GetDiagramBlueAmountElement()
        {
            return Find.Element(By.CssSelector("li.product-summary__details-item--blue"));
        }

        public int GetDiagramMonthPayment()
        {
            var element = GetDiagramBlackPaymentElement();
            return GetAmountValueFromDiagramElement(element);
        }

        public int GetDiagramAllAmountAtEndPeriod()
        {
            var element = GetDiagramBlackPaymentElement();
            return GetAmountValueFromDiagramElement(element);
        }

        public IWebElement GetDiagramBlackPaymentElement()
        {
            return Find.Element(By.CssSelector("span.product-summary__total-value.color-black"));
        }


        public double GetDiagramCreditPercent()
        {
            var element = GetDiagramCreditPercentElement();
            return Convert.ToDouble(element.Text.Replace("%", ""));
        }

        public IWebElement GetDiagramCreditPercentElement()
        {
            return Find.Element(By.CssSelector("span.product-summary__total-value.color-green"));
        }


        private int GetAmountValueFromDiagramElement(IWebElement element)
        {
            var amountStr = element.FindElement(By.CssSelector("span > span:nth-child(1)")).Text;
            amountStr = amountStr.Replace(" ", "").Replace("₽", "").Replace("$", "").Replace("€", "");

            return Convert.ToInt32(amountStr);
        }

        public void SelectFilterInitialAmount(string currencySymbol)
        {
            // currently selected currency in list (main control)
            Find.Elements(By.CssSelector("div.ik_select_link.select-control-inner--currency-link")).Last()
                .Click();

            var periodElement = GetDriver().WaitAllElementsAndReturnExpected(
                By.CssSelector("div.select-control-inner--currency-dd li.ik_select_option > span"),
                element => element.Displayed && element.Text == currencySymbol);

            periodElement.Click();

            //Ожидаине обновления диаграммы.
            WaitUntilPageIsLoaded();
            WaitUntilOverlay();
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

            //Ожидаине обновления диаграммы.
            WaitUntilPageIsLoaded();
            WaitUntilOverlay();
        }

        public IEnumerable<string> GetDiagramAmountCurrency()
        {
            var element = Find.Element(By.CssSelector("div.product-inside__calculator-col--thin"));
            var currencyElems = element.FindElements(By.CssSelector("span.currency-symbol")).Select(x => x.Text);

            return currencyElems;
        }


        public IWebElement GetSelectedPeriodElement()
        {
            var element = Find.Element(By.CssSelector("div.product-inside__calculator-col--controls"));

            return element.FindElement(By.CssSelector("div.select-control-inner-link > div"));
        }

        public virtual void SelectFilterPeriod(string periodName)
        {
            // expand menu - click on  currently selected period in list (main control)
            GetSelectedPeriodElement().Click();

            var periodElement = GetDriver()
                .FindElements(By.CssSelector("div.select-control-inner-dd li.ik_select_option > span"))
                .FirstOrDefault(element => element.Displayed && element.Text == periodName);

            periodElement.Click();

            //Ожидаине обновления диаграммы.
            WaitUntilPageIsLoaded();
            WaitUntilOverlay();
        }


        public ConditionsBlockAtDetailsPageComponent GetConditionsBlock
        {
            get { return GetComponent<ConditionsBlockAtDetailsPageComponent>(); }
        }


        public SpecialPropositionListingComponent<PropositionItem> GetSpecialPropositionListingComponent()
        {
            return GetComponent<SpecialPropositionListingComponent<PropositionItem>>();
        }


        public BankPropositionComponent<PropositionItem> GetFirstItemWithGroups()
        {
            return
                GetSpecialPropositionListingComponent()
                    .GetPropositionsResult()
                    .FirstOrDefault(item => item.GetResultGroup() != null);
        }
    }
}