using System;
using Kepler.Integration.Common.UI.Page.Common;
using OpenQA.Selenium;

namespace Kepler.Integration.Common.UI.Page.UserAccount
{
    public class BaseUserAccountTabPage : CommonPage
    {
        public void SwitchToTab()
        {
            var tabSelector = By.CssSelector("ul.secondary-menu > li");
            WaitUntilElementInvisible(tabSelector, TimeSpan.FromSeconds(10));
        }
    }
}