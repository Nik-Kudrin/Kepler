using System;
using System.Collections.Generic;
using System.Linq;
using Castle.Core.Internal;
using Kepler.Integration.Common.UI.Page.Search;
using Kepler.Integration.Common.UI.SelenoExtension;
using OpenQA.Selenium;

namespace Kepler.Integration.Common.UI.Page.Common
{
    public abstract class BasePage : TestStack.Seleno.PageObjects.Page
    {
        protected string[] modelPropertyBlackList = ModelProperties.modelPropertyBlackList;

        public BasePage WaitUntilPageIsLoaded()
        {
            WaitFor.AjaxCallsToComplete(TimeSpan.FromSeconds(60));
            return this;
        }

        public IWebDriver GetDriver()
        {
            return Browser;
        }

        public ScrollComponent Scroll()
        {
            return new ScrollComponent(Browser);
        }

        public SliderComponent Slider()
        {
            return new SliderComponent(Browser);
        }

        public SearchComponent GetPageSearchComponent()
        {
            return GetComponent<SearchComponent>();
        }


        public string GetPageDescription()
        {
            return Find.Element(By.Name("description")).GetAttribute("content");
        }

        public IEnumerable<string> GetPageKeywords()
        {
            var keywords = Find.Element(By.Name("keywords")).GetAttribute("content");
            return keywords.Split(new[] {','}, StringSplitOptions.RemoveEmptyEntries).Select(_ => _.Trim());
        }

        public IWebElement GetLinkElementByUrl(string url)
        {
            return Find.OptionalElement(By.CssSelector(String.Format("a[href=\"{0}\"]", url)));
        }

        public object GetNewsBlock(string blockName)
        {
            var blockElement = ((IJavaScriptExecutor) GetDriver())
                .ExecuteScript("return $(\"h2 a[href='/novosti/strakhovanie/']:contains('Новости')\").parent().parent()");

            return blockElement;
        }

        public IWebElement GetPopularBlock()
        {
            return Find.OptionalElement(By.CssSelector("div#navigationBlock"));
        }


        public void ChangeLocationInMenuFromFullList(string cityName)
        {
            var locationSelectComponent = GetComponent<LocationSelectInMenuComponent>();
            locationSelectComponent.OpenChangeLocationDialog();
            locationSelectComponent.ExpandFullListCities();
            locationSelectComponent.ClickCityFromFullList(cityName);

            WaitUntilPageIsLoaded();
        }

        public string GetCurrentLocationNameInMenu()
        {
            var locationName = "";

            var locationSelectComponent = GetComponent<LocationSelectInMenuComponent>();
            locationName = locationSelectComponent.GetCurrentLocationName();

            return locationName;
        }


        public void WaitUntilElementVisible(By selector, TimeSpan? timeout = null)
        {
            if (timeout == null)
                timeout = TimeSpan.FromSeconds(35);

            GetDriver().WaitUntilElementIsVisible(selector, timeout.Value);
        }

        public void WaitUntilElementInvisible(By selector, TimeSpan? timeout = null)
        {
            timeout = timeout ?? TimeSpan.FromSeconds(20);

            GetDriver().WaitUntilElementIsVisible(selector, timeout.Value, true);
        }


        public void WaitUntilOverlay()
        {
            var overlaySelector = By.CssSelector("div.info-bubbles__bubble__title");
            WaitUntilElementVisible(overlaySelector);
        }


        public bool IsElementExistOnPage(TestStack.Seleno.PageObjects.Locators.By.jQueryBy selector,
            TimeSpan? timeout = null)
        {
            if (!timeout.HasValue)
                timeout = TimeSpan.FromSeconds(30);

            GetDriver().Manage().Timeouts().ImplicitlyWait(timeout.Value);

            var findResult = !Find.Elements(selector, timeout.Value).IsNullOrEmpty();

            GetDriver().Manage().Timeouts().ImplicitlyWait(Config.Timeout);

            return findResult;
        }

        public bool IsElementExistOnPage(By selector, TimeSpan? timeout = null)
        {
            if (!timeout.HasValue)
                timeout = TimeSpan.FromSeconds(30);

            GetDriver().Manage().Timeouts().ImplicitlyWait(timeout.Value);

            var findResult = !Find.Elements(selector, timeout.Value).IsNullOrEmpty();

            GetDriver().Manage().Timeouts().ImplicitlyWait(Config.Timeout);

            return findResult;
        }

        public bool IsTextExistOnPage(string expectedText, TimeSpan? timeout = null)
        {
            if (!timeout.HasValue)
                timeout = TimeSpan.FromSeconds(30);

            var elements =
                Find.Elements(
                    TestStack.Seleno.PageObjects.Locators.By.jQuery(String.Format(":contains('{0}')", expectedText)),
                    timeout.Value);

            return !elements.IsNullOrEmpty();
        }
    }
}