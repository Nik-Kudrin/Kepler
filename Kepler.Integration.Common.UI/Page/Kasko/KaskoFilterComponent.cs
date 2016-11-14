using System.Collections.Generic;
using System.Linq;
using Kepler.Integration.Common.UI.Page.Common;
using OpenQA.Selenium;

namespace Kepler.Integration.Common.UI.Page.Kasko
{
    public class KaskoFilterComponent : CommonComponent
    {
        public void ClickOpenFilters()
        {
            var additionalFiltetsFormSelector = By.CssSelector("form > div.details-control");
            var additionalFiltetsForm = Find.Element(additionalFiltetsFormSelector);

            var buttonSelecotor = By.CssSelector("div.details-control-btn");
            var webElement = additionalFiltetsForm.FindElement(buttonSelecotor);

            webElement.Click();
        }

        public void SetFilters(List<string> needFilters)
        {
            var additionalFiltetsFormSelector = By.CssSelector("form > div.details-control");
            var additionalFiltetsForm = Find.Element(additionalFiltetsFormSelector);


            var webElementsFromList =
                additionalFiltetsForm.FindElements(By.CssSelector("div.checkbox-control > label > span"));

            foreach (var needFilter in needFilters)
            {
                var webElement = webElementsFromList.FirstOrDefault(element => element.Text.Contains(needFilter));
                webElement.Click();
            }
        }
    }
}