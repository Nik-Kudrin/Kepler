using OpenQA.Selenium;

namespace Kepler.Integration.Common.UI.Page.Common
{
    public class UiPopupComponent : CommonComponent
    {
        public IWebElement WaitPopupWindow()
        {
            return Find.Element(By.CssSelector("div.ui-popup form > div.efb_html > img"));
        }
    }
}