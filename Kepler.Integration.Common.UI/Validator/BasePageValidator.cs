using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Kepler.Integration.Common.UI.Page.Common;
using NLog;
using TestStack.Seleno.Extensions;

namespace Kepler.Integration.Common.UI.Validator
{
    public class BasePageValidator<T> where T : CommonPage
    {
        protected virtual T Page { get; set; }
        protected Logger LOG { get; set; }

        public BasePageValidator(T page)
        {
            this.Page = page;
            LOG = LogManager.GetLogger(this.GetType().UnderlyingSystemType.Name);
        }

        public void ValidateTextExistOnPage(string expectedTextOnPage)
        {
            var textExist = Page.IsTextExistOnPage(expectedTextOnPage);
            textExist.Should().
                BeTrue(String.Format("Страница должна содержать ожидаемый текст '{0}'", expectedTextOnPage));
        }

        public void ValidateTextDontExistOnPage(string expectedTextOnPage)
        {
            var textExist = Page.IsTextExistOnPage(expectedTextOnPage, TimeSpan.FromSeconds(5));
            textExist.Should().
                BeFalse(String.Format("Страница НЕ должна содержать текст '{0}'", expectedTextOnPage));
        }

        public void ValidateTextExistOnPage(IEnumerable<string> expectedTextOnPage)
        {
            expectedTextOnPage.Each(ValidateTextExistOnPage);
        }

        public void ValidateTextDontExistOnPage(IEnumerable<string> expectedTextOnPage)
        {
            expectedTextOnPage.Each(ValidateTextDontExistOnPage);
        }

        public void ValidateAnyOfTextExistOnPage(IEnumerable<string> expectedTextOnPage)
        {
            expectedTextOnPage.Any(_ => Page.IsTextExistOnPage(_))
                .Should()
                .BeTrue(String.Format("Страница должна содержать хотя бы один из ожидаемых текстов '{0}'",
                    string.Join(", ", expectedTextOnPage)));
        }

        public void ValidateLinkExistOnPage(string linkUrl)
        {
            Page.GetLinkElementByUrl(linkUrl)
                .Should().NotBeNull(String.Format("Ссылка '{0}' должна присутствовать на странице", linkUrl));
        }

        public void ValidateLinkExistOnPage(IEnumerable<string> expectedLinks)
        {
            expectedLinks.Each(ValidateLinkExistOnPage);
        }

        public void ValidateBlockExistOnPage(string blockName)
        {
            Page.GetNewsBlock(blockName)
                .Should().NotBeNull(String.Format("Блок {0} должен присутствовать на странице", blockName));
        }


        public void ValidateIsPopularBlockExistOnPage()
        {
            Page.GetPopularBlock()
                .Should().NotBeNull("Блок 'Популярное' должен присутстовать на странице");
        }

        public void ValidatePageUrlEndWith(string expectedUrlEnding, string becauseMessage = "")
        {
            Page.Url.Should().EndWith(expectedUrlEnding, becauseMessage);
        }
    }
}