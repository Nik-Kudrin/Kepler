using FluentAssertions;
using Kepler.Integration.Common.UI.SelenoExtension;
using OpenQA.Selenium;

namespace Kepler.Integration.Common.UI.Validator
{
    public static class ExternalPageValidator
    {
        public static void ValidateRedirectToExternalLink(IWebDriver driver)
        {
            driver.SwitchToNewTab();

            var currentUrl = driver.Url.ToLowerInvariant();

            //TODO подумать на будущее как можно иначе проверять редирект на внешнюю страницу 
            currentUrl.Should()
                .NotContain(Config.ServerUnderTest, "В названии url банка есть строка строка  " + Config.ServerUnderTest);

            driver.CloseAnotherTabs();
        }
    }
}