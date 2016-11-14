using System;
using System.Linq;
using Kepler.Integration.Common.UI.Page.Common;
using OpenQA.Selenium;

namespace Kepler.Integration.Common.UI.Page.Bank
{
    public class FilterPropositionTabComponent : CommonComponent
    {
        // Just for scrolling 
        public void Init()
        {
            Element = Find.Element(By.CssSelector("form.results-container-header "));
        }

        public void ClickFilterTab(FilterTab filterTab)
        {
            var tabName = ConvertTabEnumToString(filterTab);

            Element = Find.Elements(
                By.CssSelector("form.results-container-header > ul.filters-container li > span span"))
                .FirstOrDefault(item => item.Text == tabName);

            Scroll().ScrollToElement(Element);
            Element.Click();

            WaitUntilListingOverlayIsDisplayed();
        }

        public enum FilterTab
        {
            AllBanks,
            Top50,
            Top30,
            Top10
        }

        public string ConvertTabEnumToString(FilterTab filterTab)
        {
            switch (filterTab)
            {
                case FilterTab.AllBanks:
                    return "Все банки";
                case FilterTab.Top50:
                    return "Топ 50";
                case FilterTab.Top30:
                    return "Топ 30";
                case FilterTab.Top10:
                    return "Топ 10";
                default:
                    throw new NotImplementedException();
            }
        }
    }
}