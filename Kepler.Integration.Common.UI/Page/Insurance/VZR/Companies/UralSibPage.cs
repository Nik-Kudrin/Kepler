using System;
using System.Web;
using FluentAssertions;
using OpenQA.Selenium;

namespace Kepler.Integration.Common.UI.Page.Insurance.VZR.Companies
{
    public class UralSibPage : BaseInsurancePage
    {
        public virtual UralSibPage WaitUntilPageIsLoaded()
        {
            IsElementExistOnPage(By.CssSelector("div.kasko_payment-selected-right > div.kasko_payment-selected-title"), TimeSpan.FromSeconds(45));
            return this;
        }

        public override void ValidatePageElement(string policyPrice)
        {
            var price = Find.Element(By.CssSelector("div.kasko_payment-selected-right > div.kasko_payment-selected-title"))
                .Text.Trim().Replace(" ", "");
            price = HttpUtility.HtmlDecode(price);

            price.Should()
                .Contain(policyPrice, string.Format("Страница оплаты полиса дожна содержать правильную цену - {0} rub", policyPrice));
        }
    }
}