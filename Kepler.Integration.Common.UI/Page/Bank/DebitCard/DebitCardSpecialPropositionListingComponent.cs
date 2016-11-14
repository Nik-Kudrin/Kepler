using System;
using System.Collections.Generic;
using System.Linq;
using Kepler.Integration.Common.UI.Page.Bank.Product;
using TestStack.Seleno.PageObjects.Locators;

namespace Kepler.Integration.Common.UI.Page.Bank.DebitCard
{
    public class DebitCardSpecialPropositionListingComponent : SpecialPropositionListingComponent<DebitCardPropositionItem>
    {
        public new IEnumerable<DebitCardPropositionComponent> GetPropositionsResult()
        {
            var specialResultElements = Find.Elements(By.CssSelector(
                "div.results-container--special-offers > div.result-card"),
                TimeSpan.FromSeconds(3)).Where(item => item.Text.Contains("Спонсор раздела") == false);

            return InitResultComponents<DebitCardPropositionComponent>(specialResultElements);
        }
    }
}