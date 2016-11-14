using System.Linq;
using System.Threading;
using Kepler.Integration.Common.UI.SelenoExtension;
using OpenQA.Selenium;

namespace Kepler.Integration.Common.UI.Page.Common
{
    public class LocationSelectCommonComponent : CommonComponent
    {
        public void ExpandFullListCities()
        {
            var buttonShowFullList = By.CssSelector("div.location-container li.location-choose-full-btn");

            GetDriver().WaitUntilElementInvisible(buttonShowFullList);
            Find.Element(buttonShowFullList).Click();
        }

        public void ClickCityFromFullList(string cityName)
        {
            GetDriver().WaitUntilElementInvisible(By.CssSelector("div.LocationAllCities"));
            var citiesSelector = By.CssSelector("div.LocationAllCities ul.locations-list-by-letter > li > a");

            var city = Find.Elements(citiesSelector)
                .FirstOrDefault(element => element.Text == cityName);
            Scroll().ScrollToElement(city);
            city.Click();
        }

        public void ClickCityFromShortList(string cityName)
        {
            var citiesSelector = By.CssSelector("ul.location-choose-frequent li > a");

            var city = Find.Elements(citiesSelector)
                .FirstOrDefault(element => element.Text == cityName);
            city.Click();
        }

        public void SelectCityFromSearch(string cityName)
        {
            var searchBoxSelector = By.CssSelector("input.tt-location-input");
            var searchBox = Find.Element(searchBoxSelector);
            searchBox.SendKeys(cityName);

            Thread.Sleep(1000);

            var searchResultsSelector = By.CssSelector("div.search-result > strong");

            var searchResult =
                Find.Elements(searchResultsSelector).FirstOrDefault(element => element.Text.Contains(cityName));


            Scroll().ScrollToElement(searchResult);
            searchResult.Click();
        }
    }
}