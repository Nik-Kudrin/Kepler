using System;
using System.Threading;
using Kepler.Integration.Common.UI.DataGenerator;
using Kepler.Integration.Common.UI.Page.Common;
using Kepler.Integration.Common.UI.SelenoExtension;
using OpenQA.Selenium;

namespace Kepler.Integration.Common.UI.Page.Insurance
{
    public class InsuranceOrderPage : BasePage
    {
        public InsuranceOrderPage FillOrderDataAndSubmit(ClientInfoModel infoModel)
        {
            WaitFor.AjaxCallsToComplete(TimeSpan.FromSeconds(60));

            var nameField = Find.Element(By.Id("Name"));
            nameField.Clear();
            nameField.SendKeys(infoModel.Name);

            var emailField = Find.Element(By.Id("Email"));
            emailField.Clear();
            emailField.SendKeys(infoModel.Email);

            var phoneElement = Find.Element(By.CssSelector("input#Phone"));

            phoneElement.SendKeys(Keys.End);
            phoneElement.SendKeys(Keys.Control + "a");
            phoneElement.SendKeys(Keys.Delete);

            phoneElement.SendKeys(infoModel.Phone);

            var buttonSelector =
                By.CssSelector("div.order-form__fields div.order-form__fields__bottom input[type=\"submit\"][value=\"Отправить заказ\"]");

            Thread.Sleep(1000);

            Browser.WaitAndFindElement(buttonSelector, TimeSpan.FromSeconds(30)).Click();

            return this;
        }
    }
}