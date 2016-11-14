using Kepler.Integration.Common.UI.Page.Common;
using Kepler.Integration.Common.UI.SelenoExtension;
using OpenQA.Selenium;

namespace Kepler.Integration.Common.UI.Page.Bank.CreditCard
{
    public class CardCalculatorComponent : CommonComponent
    {
        private void Init()
        {
            Element = Find.Element(By.CssSelector("div.serp-calculator-inner-container"));
        }

        public void TypeAmountInAverageMonthlyExpenses(int averageMonthlyExpenses)
        {
            var averageMonthlyExpensesElement =
                Find.Element(By.CssSelector("div.calculator-controls-container div.slider-control > input[name=\"AverageMonthlyExpenses\"]"));
            averageMonthlyExpensesElement.TypeText(averageMonthlyExpenses.ToString());
        }

        public CardCalculatorTabComponent CalculatorTab
        {
            get { return GetComponent<CardCalculatorTabComponent>(); }
        }

        public new CardAdditionalFilterComponent AdditionalFilters
        {
            get { return GetComponent<CardAdditionalFilterComponent>(); }
        }

        public BrandingSponsorLogoComponent BrandingSponsorLogo
        {
            get
            {
                var component = GetComponent<BrandingSponsorLogoComponent>();
                return component.Element == null ? null : component;
            }
        }

        public class BrandingSponsorLogoComponent : CommonComponent
        {
            private void Init()
            {
                Element = Find.OptionalElement(By.CssSelector("div.branding-logo-outer-container > a"));
            }

            public string GetLink()
            {
                Init();
                return Element == null ? null : Element.GetAttribute("href");
            }

            public void Click()
            {
                Init();
                if (Element != null)
                    Element.Click();
            }
        }
    }
}