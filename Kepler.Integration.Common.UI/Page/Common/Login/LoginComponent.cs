using System;
using OpenQA.Selenium;

namespace Kepler.Integration.Common.UI.Page.Common.Login
{
    public class LoginComponent : CommonPage
    {
        public LoginComponent OpenLoginPopUp()
        {
            Find.Element(By.CssSelector("div.ProfileBtn")).Click();
            Find.OptionalElement(By.CssSelector("a.LoginFormButton")).Click();

            WaitUntilLoginPopUpInvisible();
            return this;
        }

        public LoginFormPage SwitchToLoginForm()
        {
            Find.Element(By.CssSelector("div.login-popup-form > ul.login-popup-tabs > li[data-action=\"LoginPopupSingin\"]"))
                .Click();
            return GetComponent<LoginFormPage>();
        }

        public RegisterFormPage SwitchToRegisterForm()
        {
            Find.Element(By.CssSelector("div.login-popup-form > ul.login-popup-tabs > li[data-action=\"LoginPopupSingup\"]"))
                .Click();
            return GetComponent<RegisterFormPage>();
        }

        public void WaitUntilLoginPopUpInvisible()
        {
            WaitUntilElementInvisible(By.CssSelector("sravni-auth-app"), TimeSpan.FromSeconds(5));
        }
    }
}