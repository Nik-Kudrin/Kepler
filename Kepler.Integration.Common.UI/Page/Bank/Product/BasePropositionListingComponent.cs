using System.Collections.Generic;
using Kepler.Integration.Common.UI.Page.Common;
using OpenQA.Selenium;

namespace Kepler.Integration.Common.UI.Page.Bank.Product
{
    public class BasePropositionListingComponent<T> : CommonComponent where T : PropositionItem, new()
    {
        public IEnumerable<BankPropositionComponent<T>> InitResultComponents(IEnumerable<IWebElement> propositionItems)
        {
            var resultList = new List<BankPropositionComponent<T>>();

            foreach (var resultItem in propositionItems)
            {
                var propositionResultComponent = GetComponent<BankPropositionComponent<T>>();
                propositionResultComponent.Init(resultItem);

                resultList.Add(propositionResultComponent);
            }

            return resultList;
        }

        public IEnumerable<X> InitResultComponents<X>(IEnumerable<IWebElement> propositionItems)
            where X : BankPropositionComponent<T>, new()
        {
            var resultList = new List<X>();

            foreach (var resultItem in propositionItems)
            {
                var propositionResultComponent = GetComponent<X>();
                propositionResultComponent.Init(resultItem);

                resultList.Add(propositionResultComponent);
            }

            return resultList;
        }
    }

    public enum RedirectOptions
    {
        ByMoreButton = 1,
        ByBankLogo = 2,
        ByProductName = 3,
        ByProductLogo
    }

    public enum ChangeRegionFromCalcOptions
    {
        FromShortList,
        FromFullList,
        FromSearch
    }
}