using System;
using System.Collections.Generic;
using System.Linq;
using TestStack.Seleno.PageObjects.Locators;

namespace Kepler.Integration.Common.UI.Page.Bank.Product
{
    public class SpecialPropositionListingComponent<T> : BasePropositionListingComponent<T> where T : PropositionItem, new()
    {
        public virtual IEnumerable<BankPropositionComponent<T>> GetPropositionsResult()
        {
            var specialResultElements = Find.Elements(By.CssSelector(
                "div.results-container--special-offers > div.results-container > ul > li.results-container-line"),
                TimeSpan.FromSeconds(3)).Where(item => item.Text.Contains("Спонсор раздела") == false);
            return InitResultComponents(specialResultElements);
        }

        public virtual IEnumerable<BankPropositionComponent<T>> GetSuperSpecialPropositions()
        {
            var resultsItem =
                Find.Elements(OpenQA.Selenium.By.CssSelector("ul.results-container--super-special-offers > li.results-container-line"),
                    TimeSpan.FromSeconds(1));
            return InitResultComponents(resultsItem);
        }
    }
}