using System;
using System.Collections.Generic;
using System.Linq;
using Kepler.Integration.Common.UI.Page.Bank.CreditCard;
using Kepler.Integration.Common.UI.SelenoExtension;
using OpenQA.Selenium;

namespace Kepler.Integration.Common.UI.Page.Bank.Product
{
    public interface IResult<T, M>
        where M : CardPropositionItem, new()
        where T : CardPropositionComponent<M>

    {
        IEnumerable<T> GetPropositionsResult();
    }

    public class StandardPropositionListingComponent<T> : BasePropositionListingComponent<T> where T : PropositionItem, new()
    {
        public virtual IEnumerable<BankPropositionComponent<T>> GetPropositionsResult()
        {
            var resultElements =
                Find.Elements(
                    By.CssSelector("div.standard-offers-test > .results-container > li.results-container-line"),
                    TimeSpan.FromSeconds(3)).Where(item => item.GetAttribute("class").Contains("T-Vklady-TGB") == false);

            return InitResultComponents(resultElements);
        }

        public virtual void SelectSortingType(string sortOption)
        {
            GetSelectedSortingElement().Click();

            var selectedSortingElement =
                GetDriver()
                    .WaitAllElementsAndReturnExpected(
                        By.CssSelector("div.select-control-inner--sorting-dd li.ik_select_option"),
                        element => element.Displayed && element.FindElement(By.CssSelector("span")).Text == sortOption);

            selectedSortingElement.Click();
        }

        protected IWebElement GetSelectedSortingElement()
        {
            var sortingSelectElement =
                Find.OptionalElement(By.CssSelector("div.select-control-inner--sorting div.ik_select_link_text"));
            Scroll().ScrollToElement(sortingSelectElement);

            return sortingSelectElement;
        }

        public string GetSelectedSortingType()
        {
            return GetSelectedSortingElement().Text.Trim();
        }
    }
}