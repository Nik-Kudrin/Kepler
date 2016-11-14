using System;
using System.Collections.Generic;

namespace Kepler.Integration.Common.UI.Page.Bank.Product
{
    public class BankTopListingPage : BankListingPage<PropositionItem>
    {
        public IEnumerable<BankPropositionComponent<PropositionItem>> GetTopSpecialPropositionResult()
        {
            return GetComponent<SpecialPropositionTopListingComponent<PropositionItem>>().GetPropositionsResult();
        }

        public IEnumerable<BankPropositionComponent<PropositionItem>> GetStandardPropositionResult()
        {
            return GetComponent<StandardPropositionTopListingComponent<PropositionItem>>().GetPropositionsResult();
        }

        public IEnumerable<BankTopPropositionComponent> GetStandartPropositionHeaders()
        {
            var standardPropositionsBlocks =
                Find.Elements(
                    TestStack.Seleno.PageObjects.Locators.By.jQuery(
                        "div.top-results-serp > div.results-container:not(div.results-container--special-offers)"),
                    TimeSpan.FromSeconds(3));

            var resultList = new List<BankTopPropositionComponent>();

            foreach (var item in standardPropositionsBlocks)
            {
                var standartProposition = GetComponent<BankTopPropositionComponent>();
                standartProposition.Init(item);

                resultList.Add(standartProposition);
            }

            return resultList;
        }
    }
}