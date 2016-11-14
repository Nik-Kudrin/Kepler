using Kepler.Integration.Common.UI.Page.Bank.Product;
using OpenQA.Selenium;

namespace Kepler.Integration.Common.UI.Page.Bank.CreditCard
{
    public class CardPropositionComponent<T> : BankPropositionComponent<T> where T : CardPropositionItem, new()
    {
        public override void Init(IWebElement element)
        {
            PropositionItem = GetComponent<T>();
            PropositionItem.Init(element);
        }

        public new T GetTopItemResult()
        {
            var topElement = PropositionItem.Element.FindElement(By.CssSelector("div.T-Proposition"));
            var topElementComponent = GetComponent<T>();
            topElementComponent.Init(topElement);

            return topElementComponent;
        }
    }
}