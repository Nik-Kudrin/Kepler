using System.Collections.Generic;
using System.Linq;
using Kepler.Integration.Common.UI.Page.Common;
using OpenQA.Selenium;

namespace Kepler.Integration.Common.UI.Page.Kasko
{
    public class RadioControlComponent : CommonComponent
    {
        
        public void ClickByElementFromList(string elem)
        {
            var webElementsFromList = FindAllElementsInRadioControl();
            var webElement = webElementsFromList.FirstOrDefault(element => element.Text.Contains(elem));

            if (webElement != null) webElement.Click();
        }


        public IEnumerable<IWebElement> FindAllElementsInRadioControl()
        {
            var webElementsFromList =
                Find.Element(By.CssSelector(".radio-control-inner")).FindElements(By.CssSelector("ul > li > label"));
            return webElementsFromList;
        }
    }
}