using Kepler.Integration.Common.UI.DataGenerator;
using Kepler.Integration.Common.UI.SelenoExtension;
using OpenQA.Selenium;

namespace Kepler.Integration.Common.UI.Page.Common.Login
{
    public class ChangePasswordPage : CommonPage
    {
        public void FillPasswordsFields(ClientInfoModel clientInfo)
        {
            var passField = Find.Element(By.CssSelector("input#Password"));
            var passConfirmationField = Find.Element(By.CssSelector("input#PasswordConfirmation"));

            passField.TypeText(clientInfo.Password);
            passConfirmationField.TypeText(clientInfo.Password);
        }

        public void ClickSubmitButton()
        {
            var submitButton = Find.Element(By.CssSelector("div.user-forms > div.user-forms__container__buttons > input[type=\"submit\"]"));
            submitButton.Click();
        }
    }
}