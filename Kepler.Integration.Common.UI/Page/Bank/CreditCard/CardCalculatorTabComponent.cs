using System.Linq;
using Kepler.Integration.Common.UI.Page.Common;
using OpenQA.Selenium;

namespace Kepler.Integration.Common.UI.Page.Bank.CreditCard
{
    public class CardCalculatorTabComponent : CommonComponent
    {
        private void Init()
        {
            Element = Find.Element(By.CssSelector("div.radio-control-inner"));
        }

        public void ClickTab(string tabName)
        {
            Init();
            var tab = Element.FindElements(By.CssSelector("li span")).FirstOrDefault(_ => _.Text.Trim() == tabName);
            tab.Click();
        }
    }
}