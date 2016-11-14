using System;
using System.Threading;
using Kepler.Integration.Common.UI.Page.Common;
using Kepler.Integration.Common.UI.SelenoExtension;
using OpenQA.Selenium;

namespace Kepler.Integration.Common.UI.Page.Search
{
    public class SearchComponent : CommonComponent
    {
        private string _searchFieldSelector = "input[name='search_query']";


        public SearchComponent ClickSearchButton()
        {
            Find.Element(By.CssSelector("a.SearchBtn")).Click();
            return this;
        }

        public SearchComponent EnterTextInSearchField(string text)
        {
            var searchField = Find.Element(By.CssSelector(_searchFieldSelector), TimeSpan.FromSeconds(60));
            searchField.SendKeys(text);
            searchField.SendKeys(Keys.Home);

            return this;
        }

        public CommonPage ClickItemFromSearchSuggest(string expectedItemTextInSuggest)
        {
            var expectedElement = Browser.WaitAllElementsAndReturnExpected(By.CssSelector("div.tt-dataset-search div"),
                element => element.Displayed && element.Text == expectedItemTextInSuggest);
            Thread.Sleep(1000); // because I don't know why

            expectedElement.Click();

            return GetComponent<CommonPage>();
        }

        public SearchResultPage TypeEnterInSearchField()
        {
            var searchField = Find.Element(By.CssSelector(_searchFieldSelector), TimeSpan.FromSeconds(60));
            searchField.SendKeys(Keys.Enter);

            return GetComponent<SearchResultPage>();
        }
    }
}