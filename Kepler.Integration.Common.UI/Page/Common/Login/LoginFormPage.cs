using Kepler.Integration.Common.UI.DataGenerator;
using Kepler.Integration.Common.UI.SelenoExtension;
using OpenQA.Selenium;

namespace Kepler.Integration.Common.UI.Page.Common.Login
{
    public class LoginFormPage : CommonPage, ILoginFieldFiller
    {
        private By _loginFormSelector = By.CssSelector("div.login-popup-form > div.login-popup-content > form#LoginPopupSingin");
        private By _forgotPassFormSelector = By.CssSelector("div.login-popup-form > div.login-popup-content > form#LoginPopupForgot");

        private IWebElement GetForm()
        {
            return Find.Element(_loginFormSelector);
        }

        public void FillUserField(ClientInfoModel clientInfo)
        {
            var loginForm = GetForm();

            var emailField = loginForm.FindElement(By.CssSelector("div.login-popup-row-field > div > input[name=\"Email\"]"));
            emailField.TypeText(clientInfo.Email);

            var passwordField = loginForm.FindElement(By.CssSelector("div.login-popup-row-field--last > div > input"));
            passwordField.TypeText(clientInfo.Password);
        }

        public void ClickSubmitButton()
        {
            var submitButton =
                GetDriver().WaitAllElementsAndReturnExpected(
                    By.CssSelector("div.login-popup-form > div.login-popup-content > form#LoginPopupSingin div > input.LoginPopupSinginSubmit"),
                    item => item.Enabled);

            submitButton.Click();
        }

        public LoginFormPage ClickRestorePasswordButton()
        {
            var restoreButton = GetForm().FindElement(By.CssSelector("div.login-popup-row-forgot-password > a"));
            restoreButton.Click();

            GetDriver().WaitUntilElementInvisible(_forgotPassFormSelector);
            return this;
        }

        public void FillRestorePassForm(ClientInfoModel clientInfo)
        {
            var forgotPassForm = Find.Element(_forgotPassFormSelector);
            var emailField = forgotPassForm.FindElement(By.CssSelector("div.login-popup-row-field--last > div > input"));
            emailField.TypeText(clientInfo.Email);

            var submitButton = GetDriver().WaitAllElementsAndReturnExpected(
                By.CssSelector("div.login-popup-form > div.login-popup-content > form#LoginPopupForgot > div > input.LoginPopupForgotSubmit"),
                item => item.Enabled);

            submitButton.Click();
        }
    }
}