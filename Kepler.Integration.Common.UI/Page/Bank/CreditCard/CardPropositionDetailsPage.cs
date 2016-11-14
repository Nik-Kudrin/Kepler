using Kepler.Integration.Common.UI.Page.Bank.Product;
using Kepler.Integration.Common.UI.Page.Common;
using OpenQA.Selenium;

namespace Kepler.Integration.Common.UI.Page.Bank.CreditCard
{
    public class CardPropositionDetailsPage : BankPropositionDetailsPage
    {
        public override IWebElement ClickGreenLinkGoToSiteAndGetPopUp()
        {
            Find.Element(By.Id("LeedButton")).Click();
            return GetComponent<UiPopupComponent>().WaitPopupWindow();
        }

        public override string GetBankNameFromLogo()
        {
            return Find.Element(By.CssSelector("div.organization-card__header__title__value")).Text.Trim();
        }

        public override IWebElement GetProductNameElement()
        {
            return Find.Element(By.CssSelector("h1.content-block__title"));
        }
    }
}