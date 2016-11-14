using Kepler.Integration.Common.UI.Page.Common;
using OpenQA.Selenium;

namespace Kepler.Integration.Common.UI.Page.Kasko.PopUps
{
    public class ConfirmEmailPopupComponent : CommonComponent
    {
        public const string ComponentSelector = "div.react-popup > div.react-popup__form";

        public void ClickConfirm()
        {
            var selector = By.CssSelector("div.react-popup__form > div > form > span > button");
            var confirmPopup = Find.Element(By.CssSelector(ComponentSelector));

            confirmPopup.FindElement(selector).Click();
        }


        public void EnterEmail(string email)
        {
            var selector =
                By.CssSelector("div.react-popup__content > form > div:nth-child(5) > div.text-control > div > input");
            Scroll().ScrollToElement(selector);
            var confirmPopup = Find.Element(By.CssSelector(ComponentSelector));

            var codeField = confirmPopup.FindElement(selector);
            codeField.SendKeys(email);
        }
    }
}