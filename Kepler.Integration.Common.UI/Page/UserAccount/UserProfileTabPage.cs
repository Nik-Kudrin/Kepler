using OpenQA.Selenium;

namespace Kepler.Integration.Common.UI.Page.UserAccount
{
    public class UserProfileTabPage : BaseUserAccountTabPage
    {
        public void WaitUntilUserPopUpMessageInvisible()
        {
            WaitUntilElementInvisible(By.CssSelector("div#UserProfileMessages"));
        }

        public string GetPopUpHeaderName()
        {
            return Find.Element(By.CssSelector("div#UserProfileMessages > div.login-popup-row-text > div.heading")).Text;
        }
    }
}