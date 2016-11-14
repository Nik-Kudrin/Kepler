using System.Collections.Generic;
using System.Linq;
using Kepler.Integration.Common.UI.Page.Common;
using Kepler.Integration.Common.UI.SelenoExtension;
using OpenQA.Selenium;

namespace Kepler.Integration.Common.UI.Page.Search
{
    public class SearchResultPage : CommonPage
    {
        public IEnumerable<IWebElement> Results { get; set; }

        public SearchResultPage WaitUntilResultListAppeared()
        {
            Browser.WaitAndFindElement(By.CssSelector("div.gsc-results"));

            return this;
        }

        public SearchResultPage CollectSearchResult()
        {
            Results = Find.Elements(By.CssSelector("table.gsc-table-result div a.gs-title"));

            return this;
        }

        public IWebElement FindItemInResult(string itemName)
        {
            var maxLenghtSearchText = itemName.Length > 45 ? 45 : itemName.Length;

            return Results.FirstOrDefault(item => item.Text.Substring(0, maxLenghtSearchText) == itemName.Substring(0, maxLenghtSearchText));
        }
    }
}