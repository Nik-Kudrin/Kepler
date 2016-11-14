using System;
using FluentAssertions;
using OpenQA.Selenium;

namespace Kepler.Integration.Common.UI.Page.Insurance.VZR.Companies
{
    public class AlphaStrakhovaniePage : BaseInsurancePage
    {
        public virtual AlphaStrakhovaniePage WaitUntilPageIsLoaded()
        {
            IsElementExistOnPage(By.CssSelector("div.price"), TimeSpan.FromSeconds(45));
            return this;
        }

        public override void ValidatePageElement(string policyPrice = "1000")
        {
            Find.Element(By.CssSelector("div.price")).Text.Trim().Replace(" ", "")
                .Should()
                .Contain(policyPrice, string.Format("Страница оплаты полиса дожна содержать правильную цену - {0} rub", policyPrice));

            var infoElementText = Find.Element(By.CssSelector("div.info")).Text.Trim();
            infoElementText.Should().Contain("Полис:");
            infoElementText.Should().Contain("CAS-SRV1000091");

            Find.Element(By.CssSelector("button.as-button-grey")).Text.Trim()
                .Should().BeEquivalentTo("Оплатить", "Страница оплаты полиса должна содержать кнопку оплаты");
        }
    }
}