using System;
using System.Collections.Generic;
using Kepler.Integration.Common.UI.Page.Common;
using Kepler.Integration.Common.UI.Validator;
using OpenQA.Selenium;

namespace Kepler.Integration.Common.UI.Page.Publication
{
    public class PublicationListPage : CommonPage
    {
        public PublicationPageValidator GetPageValidator()
        {
            return new PublicationPageValidator(this);
        }


        public IEnumerable<IWebElement> GetPreviewPublicationItems(string url)
        {
            return Find.Elements(By.CssSelector(GetSelectorForArticlePreview(url)), TimeSpan.FromSeconds(30));
        }

        public void ClickShowMoreArticleButton()
        {
            var element = Find.Element(By.CssSelector("div.NewsShowMore"));
            Scroll().ScrollToElement(element);
            element.Click();
        }

        private string GetSelectorForArticlePreview(string pageUrl)
        {
            if (pageUrl == "/novosti/rubrika/novosti/")
                return "li.article-preview";

            return "div.article-preview";
        }
    }
}