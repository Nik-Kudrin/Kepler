using Kepler.Integration.Common.UI.SelenoExtension;
using OpenQA.Selenium;

namespace Kepler.Integration.Common.UI.Page.Common
{
    public class LocationSelectInCalcComponent : LocationSelectCommonComponent
    {
        private By _changeLocationLink = By.CssSelector("span.serp-calculator-city-container > span");

        private IWebElement CurrentLocationInCalc
        {
            get
            {
                GetDriver().WaitUntilElementInvisible(_changeLocationLink);
                return Find.Element(_changeLocationLink);
            }
        }

        public void OpenChangeLocationDialog()
        {
            CurrentLocationInCalc.Click();
        }

        public string GetCurrentLocationName()
        {
            return CurrentLocationInCalc.Text;
        }
    }
}