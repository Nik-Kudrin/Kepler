using System;
using Kepler.Integration.Common.UI.Page.Common;
using OpenQA.Selenium;

namespace Kepler.Integration.Common.UI.Page.Kasko.PopUps
{
    public class OrderInfoPopupComponent : CommonComponent
    {
        public const string ComponentSelector = "div.react-popup > div.react-popup__form";

        public void ClickClose()
        {
            var selector = By.CssSelector(" span.react-popup__close");
            var confirmPopup = Find.Element(By.CssSelector(ComponentSelector));

            confirmPopup.FindElement(selector).Click();
        }

        public int GetOrderNumber()
        {
            var selector = "div > div > p:nth-child(2)";
            var confirmPopup = Find.Element(By.CssSelector(ComponentSelector));

            var str = confirmPopup.FindElement(By.CssSelector(selector));

            //+2 так как после знака номера еще есть пробел.
            var index = str.Text.IndexOf("№") + 2;
            //5 - это число цифр в номере заказа
            var orderNumberStr = str.Text.Substring(index, 5);

            return Int32.Parse(orderNumberStr);
        }
    }
}