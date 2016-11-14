using System;
using System.Collections.Generic;
using System.Linq;

namespace Kepler.Integration.Common.UI.Page.Bank.Product
{
    public class StandardPropositionTopListingComponent<T> : StandardPropositionListingComponent<T> where T : PropositionItem, new()
    {
        public override IEnumerable<BankPropositionComponent<T>> GetPropositionsResult()
        {
            var items = Find.Elements(TestStack.Seleno.PageObjects.Locators.By.jQuery(
                "div.top-results-serp > div.results-container:not(div.results-container--special-offers) div.result-card"),
                TimeSpan.FromSeconds(3)).Where(item => !item.GetAttribute("class").Contains("T-Vklady-TGB"));

            return InitResultComponents(items);
        }
    }
}