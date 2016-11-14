using System;
using System.Linq;
using System.Threading;
using Kepler.Integration.Common.UI.Page.Common;
using Kepler.Integration.Common.UI.Validator.BankPageValidator.Product;
using OpenQA.Selenium;

namespace Kepler.Integration.Common.UI.Page.Bank.Product
{
    public class BankListingPage<T> : CommonPage where T : PropositionItem, new()
    {
        public enum BankProductType
        {
            Credit,
            Mortgage,
            Deposit,
            CreditCard,
            DebitCard,
            AutoCredit
        }


        public BankProductType ProductType { get; set; }

        public string GetListingTitle()
        {
            return Find.Element(By.CssSelector("div.serp-calculator-heading-container h1")).Text;
        }

        public SidebarComponent SidebarComponent
        {
            get { return GetComponent<SidebarComponent>(); }
        }

        public virtual BankCalculatorComponent Calculator
        {
            get { return GetComponent<BankCalculatorComponent>(); }
        }

        public LocationSelectInCalcComponent GetComponentLocationSelectInCalcComponent()
        {
            return GetComponent<LocationSelectInCalcComponent>();
        }

        // Кнопка "Предложения всех банков"
        public BankListingPage<T> ClickCalculatorAllBanks()
        {
            return
                Navigate.To<BankListingPage<T>>(
                    By.CssSelector("div.details-control span.calculator-all-banks-btn"));
        }

        public string GetButtonTextCalculatorAllBanks()
        {
            return Find.Element(By.CssSelector("div.details-control span.calculator-all-banks-btn")).Text;
        }

        public void TypeTextInSearchBankField(string bankName)
        {
            var searchButtonSelector = By.CssSelector("div.icon--search > svg.icon-img");

            WaitUntilElementInvisible(searchButtonSelector);
            Scroll().ScrollToElement(searchButtonSelector);
            Thread.Sleep(1000);

            var searchElement = Find.Element(searchButtonSelector);

            searchElement.Click();
            var searchField = Find.Element(By.CssSelector("input[name='term']"));
            searchField.SendKeys(bankName);
        }

        public void SelectSearchSuggestOption(string searchText)
        {
            GetPageSearchComponent().ClickItemFromSearchSuggest(searchText);
        }

        public void ClearSearchBankString()
        {
            var cancelButtonSelector =
                By.CssSelector("div.company-search-container > div > span.icon--close > svg.icon-img");
            var cancelButton = Find.Element(cancelButtonSelector);

            Scroll().ScrollToElement(cancelButton);
            cancelButton.Click();
        }

        public virtual M GetSpecialPropositions<M>()
            where M : SpecialPropositionListingComponent<T>, new()
        {
            return GetComponent<M>();
        }

        public virtual M GetStandardPropositions<M>()
            where M : StandardPropositionListingComponent<T>, new()
        {
            return GetComponent<M>();
        }

        public PixelComponent<T> PixelComponent
        {
            get { return GetComponent<PixelComponent<T>>(); }
        }

        public FilterPropositionTabComponent FilterPropositionTab
        {
            get { return GetComponent<FilterPropositionTabComponent>(); }
        }

        public virtual BankPropositionListingPageValidator<BankListingPage<T>, T> GetPageValidator()
        {
            return new BankPropositionListingPageValidator<BankListingPage<T>, T>(this);
        }

        public BankPropositionComponent<T> GetFirstItemWithGroups()
        {
            return
                GetStandardPropositions<StandardPropositionListingComponent<T>>()
                    .GetPropositionsResult()
                    .FirstOrDefault(item => item.GetResultGroup() != null);
        }


        public void ClickShowMoreOffersButton()
        {
            var element = Find.Element(By.CssSelector("div.results-container div.anchor-block"));
            Scroll().ScrollToElement(element);
            element.Click();
        }

        public int GetCountPropositionsFromShowMoreButton()
        {
            var element = Find.Element(By.CssSelector("div.results-container div.anchor-block > div"));

            var allText = element.Text;
            var index = allText.LastIndexOf("из", StringComparison.Ordinal) + 3; //3 это символы "и" "з" и " "
            var countProp = Convert.ToInt32(allText.Substring(index));

            return countProp;
        }

        public IWebElement GetAmountSliderElement()
        {
            return Find.Element(
                By.CssSelector("div.slider-control > input[name='amount'] ~ div.deposit-amount-slider-container"));
        }


        public IWebElement GetInitialAmountSliderElement()
        {
            return Find.Element(
                By.CssSelector(
                    "div.slider-control > input[name='initialAmount'] ~ div.deposit-amount-slider-container"));
        }
    }
}