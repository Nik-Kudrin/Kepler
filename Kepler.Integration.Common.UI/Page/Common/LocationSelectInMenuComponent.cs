using Kepler.Integration.Common.UI.SelenoExtension;
using OpenQA.Selenium;

namespace Kepler.Integration.Common.UI.Page.Common
{
    public class LocationSelectInMenuComponent : LocationSelectCommonComponent
    {
        private By _accountMenuButton = By.CssSelector("div.ProfileBtn");
        private By _changeLocationButton = By.CssSelector("a.LocationBtn");


        public void OpenChangeLocationDialog()
        {
            var accountElement = Find.Element(_accountMenuButton);
            accountElement.Click();

            accountElement.WaitAndFindElement(_changeLocationButton, _ => _.Displayed, GetDriver());
            accountElement.FindElement(_changeLocationButton).Click();
        }

        public string GetCurrentLocationName()
        {
            var accountElement = Find.Element(_accountMenuButton);
            accountElement.Click();

            var locationSelector = By.CssSelector("a.LocationBtn > div");
            accountElement.WaitAndFindElement(locationSelector, _ => _.Displayed, GetDriver());

            return accountElement.FindElement(locationSelector).Text;
        }
    }
}