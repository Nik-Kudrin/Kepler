using System;
using System.Collections.Generic;
using System.Linq;
using Kepler.Integration.Common.UI.SelenoExtension;
using OpenQA.Selenium;

namespace Kepler.Integration.Common.UI.Page.Common
{
    public abstract class BasePage : TestStack.Seleno.PageObjects.Page
    {
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
    }
}