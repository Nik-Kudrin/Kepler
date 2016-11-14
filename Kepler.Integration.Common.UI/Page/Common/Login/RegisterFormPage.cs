using Kepler.Integration.Common.UI.DataGenerator;
using Kepler.Integration.Common.UI.SelenoExtension;
using OpenQA.Selenium;

namespace Kepler.Integration.Common.UI.Page.Common.Login
{
    public class RegisterFormPage : CommonComponent, ILoginFieldFiller
    {
        private By _formSelector = By.CssSelector("div.login-popup-form > div.login-popup-content > form#LoginPopupSingup");

        private IWebElement GetForm()
        {
            return Find.Element(_formSelector);
        }

        public void FillUserField(ClientInfoModel clientInfo)
        {
            var form = GetForm();

            var nameField = form.FindElement(By.CssSelector("div.login-popup-row-field > div > input[name=\"FirstAndLastName\"]"));
            nameField.TypeText(clientInfo.Name);

            var emailField = form.FindElement(By.CssSelector("div.login-popup-row-field > div > input[name=\"Email\"]"));
            emailField.TypeText(clientInfo.Email);

            var passwordField = form.FindElement(By.CssSelector("div.login-popup-row-field--before-text > div > input"));
            passwordField.TypeText(clientInfo.Password);

            // hack begins for  oferta Agreement Checkbox
            GetDriver().SetAttribute("\"div.login-popup-row-oferta > div > label > input\"", "checked", "true");
        }


        public void ClickSubmitButton()
        {
            var signupSubmitButton = GetDriver().WaitAllElementsAndReturnExpected(
                By.CssSelector("div.login-popup-form > div.login-popup-content > form#LoginPopupSingup > div > input.LoginPopupSingupSubmit"),
                item => item.Enabled);

            signupSubmitButton.Click();
        }
    }
}