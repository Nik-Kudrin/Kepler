using System;
using Kepler.Integration.Common.UI.Page.UserAccount;
using Kepler.Integration.Common.UI.SelenoExtension;
using OpenQA.Selenium;

namespace Kepler.Integration.Common.UI.Page.Common
{
    public class AccountMenuComponent : CommonPage
    {

        public void OpenAccountMenu()
        {
            Find.Element(By.CssSelector("div.ProfileBtn")).Click();
        }

        public UserProfileTabPage GotoUserProfilePageThroughMenu()
        {
            ExpandUserLoginMenu();
            var profileMenuItemSelector = By.CssSelector("div.login-container > div.login-menu a[href=\"/user/\"]");

            // FF alert
            try
            {
                GetDriver().SwitchTo().Alert().Accept();
            }
            catch (Exception ex)
            {
            }

            GetDriver().WaitUntilElementInvisible(profileMenuItemSelector);

            return Navigate.To<UserProfileTabPage>(profileMenuItemSelector);
        }


        public CommonPage Logout()
        {
            try
            {
                ExpandUserLoginMenu();
            }
            catch (Exception ex)
            {
                return Navigate.To<CommonPage>(Config.ServerUnderTest);
            }

            var logoutSelector = By.CssSelector("div.login-container > div.login-menu a[href=\"/logout/\"]");
            GetDriver().WaitUntilElementInvisible(logoutSelector);

            return Navigate.To<CommonPage>(logoutSelector);
        }

        private void ExpandUserLoginMenu()
        {
            var loginContainerSelector = By.CssSelector("div.login-container > a.login > span.login-name");
            GetDriver().WaitUntilElementInvisible(loginContainerSelector, TimeSpan.FromSeconds(10));

            var loginContainer = Find.Element(loginContainerSelector);
            loginContainer.Click();
        }
    }
}