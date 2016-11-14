using System;
using System.Collections.Generic;
using OpenQA.Selenium;

namespace Kepler.Integration.Common.UI.Page.Bank.Product
{
    public class SpecialPropositionTopListingComponent<T> : SpecialPropositionListingComponent<T> where T : PropositionItem, new()
    {
        public override IEnumerable<BankPropositionComponent<T>> GetPropositionsResult()
        {
            var specialResultElements =
                Find.Elements(By.CssSelector("div.results-container--special-offers ul.results-container > div"),
                    TimeSpan.FromSeconds(3));
            return InitResultComponents(specialResultElements);
        }
    }
}