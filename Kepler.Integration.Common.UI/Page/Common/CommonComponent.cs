using Kepler.Integration.Common.UI.SelenoExtension;
using OpenQA.Selenium;
using TestStack.Seleno.PageObjects;

namespace Kepler.Integration.Common.UI.Page.Common
{
    public class CommonComponent : UiComponent
    {
        public IWebElement Element { get; set; }

        public ScrollComponent Scroll()
        {
            return new ScrollComponent(Browser);
        }

        public IWebDriver GetDriver()
        {
            return Browser;
        }
    }
}