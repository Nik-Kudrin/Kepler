using Kepler.Integration.Common.UI.Page.Common;
using OpenQA.Selenium;

namespace Kepler.Integration.Common.UI.Page.Kasko.PopUps
{
    public class ConfirmPopupComponent : CommonComponent
    {
        public const string ComponentSelector =
            "div.serp-calculator-inner-container > div:nth-child(1) > div.react-popup__form";


        public void EnterConfirmCode()
        {
            const string confirmCode = "99999";

            var selector =
                By.CssSelector("div.react-popup__content > form > div:nth-child(3) > div.text-control > div > input");
            //TODO Возможно из за этого падают тесты во время запуска в teamcity
            Scroll().ScrollToElement(selector);
            var confirmPopup = Find.Element(By.CssSelector(ComponentSelector));

            var codeField = confirmPopup.FindElement(selector);
            codeField.SendKeys(confirmCode);
        }

        public void ClickConfirm()
        {
            var selector = By.CssSelector("div.react-popup__form > div > form > span > button");
            var confirmPopup = Find.Element(By.CssSelector(ComponentSelector));
            var button = confirmPopup.FindElement(selector);

            while (!button.Enabled)
            {
            }
            button.Click();
        }
    }
}