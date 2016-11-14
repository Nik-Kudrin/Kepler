using System.Collections.Generic;
using Kepler.Integration.Common.UI.Page.Common;
using OpenQA.Selenium;

namespace Kepler.Integration.Common.UI.Page.Insurance.VZR
{
    public class VzrListingPage : CommonPage
    {
        protected IEnumerable<IWebElement> _listingResutls;

        public VzrResultsComponent FindVzrResultsByInsuranceName(string insuranceName)
        {
            foreach (var element in _listingResutls)
            {
                var itemResult = GetComponent<VzrResultsComponent>();
                itemResult.Element = element;

                if (itemResult.GetItemTitle() == insuranceName)
                    return itemResult;
            }

            return null;
        }


        public VzrListingPage CollectResultsItems()
        {
            _listingResutls = Find.Elements(By.CssSelector("div.results__item"));
            return this;
        }
    }
}