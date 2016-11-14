using System;
using System.Linq;
using Castle.Core.Internal;
using Kepler.Integration.Common.UI.Page.Common;
using OpenQA.Selenium;

namespace Kepler.Integration.Common.UI.Page.Bank.Product
{
    public class BankTopPropositionComponent : CommonComponent
    {
        public IWebElement Element { get; set; }

        public void Init(IWebElement element)
        {
            Element = element;
        }

        public HeaderLabelLink GetHeaderLabelLink()
        {
            var headerComponent = GetComponent<HeaderLabelLink>();
            headerComponent.Init(Element.FindElement(By.CssSelector("div.results-container-header > div.results-container-label")));
            return headerComponent;
        }

        public class HeaderLabelLink : CommonComponent
        {
            public IWebElement Element { get; set; }
            public string LabelText { get; set; }

            public void Init(IWebElement element)
            {
                Element = element;
            }

            private IWebElement GetLabel()
            {
                return Element.FindElement(By.CssSelector("a"));
            }

            public BankListingPage<PropositionItem> ClickLabel()
            {
                Scroll().ScrollToElement(Element);
                GetLabel().Click();
                return GetComponent<BankListingPage<PropositionItem>>();
            }

            public int GetAmount()
            {
                return ParseLabelString().Item1;
            }

            public string GetPeriodName()
            {
                return ParseLabelString().Item2;
            }

            private Tuple<int, string> ParseLabelString()
            {
                if (LabelText.IsNullOrEmpty())
                    LabelText = GetLabel().Text;

                var separators = new[] {" на "};
                var splittedLabel = LabelText.Split(separators, StringSplitOptions.RemoveEmptyEntries);

                var stringAmount = splittedLabel.First().Trim().Replace(" ", "");
                if (stringAmount.Contains("$") || stringAmount.Contains("€"))
                    stringAmount = stringAmount.Remove(stringAmount.Length - 1); // Remove currency symbol

                var amount = int.Parse(stringAmount);

                var periodName = splittedLabel.Last();

                var substringPosition = periodName.IndexOf("c", StringComparison.InvariantCulture);
                if (substringPosition < 0)
                    substringPosition = periodName.IndexOf("без", StringComparison.InvariantCulture);
                if (substringPosition < 0)
                    substringPosition = periodName.Length;

                periodName = periodName.Substring(0, substringPosition).Trim().ToLowerInvariant();

                return new Tuple<int, string>(amount, periodName);
            }
        }
    }
}