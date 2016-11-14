using System;
using System.Collections.Generic;
using System.Linq;
using Kepler.Integration.Common.UI.Page.Bank.Product;
using TestStack.Seleno.PageObjects.Locators;

namespace Kepler.Integration.Common.UI.Page.Bank.CreditCard
{
    public class CreditCardSpecialPropositionListingComponent : SpecialPropositionListingComponent<CardPropositionItem>
    {
        public new IEnumerable<CreditCardPropositionComponent> GetPropositionsResult()
        {
            var specialResultElements = Find.Elements(By.CssSelector(
                "div.results-container--special-offers > div.result-card"),
                TimeSpan.FromSeconds(3)).Where(item => item.Text.Contains("Спонсор раздела") == false);

            return InitResultComponents<CreditCardPropositionComponent>(specialResultElements);
        }
    }
}