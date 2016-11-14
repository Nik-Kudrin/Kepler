using System.Linq;
using FluentAssertions;
using Kepler.Integration.Common.UI.Page.Publication;
using OpenQA.Selenium;

namespace Kepler.Integration.Common.UI.Validator
{
    public class PublicationPageValidator : BasePageValidator<PublicationListPage>
    {
        public PublicationPageValidator(PublicationListPage page) : base(page)
        {
        }

        public void ValidateBigArticleItemExistOnPage()
        {
            Page.IsElementExistOnPage(By.CssSelector("li.NewsBiggerItem"))
                .Should().BeTrue("Главный блок со статьей должен присутствовать на странице списка новостей");
        }

        public void ValidatePreviewArticleItemsExistOnPage(string url)
        {
            Page.GetPreviewPublicationItems(url)
                .Count().Should()
                .BeGreaterOrEqualTo(5, "Блоки превью статей должны присутствовать на странице списка новостей (и их кол-во должно быть больше 5)");
        }
    }
}