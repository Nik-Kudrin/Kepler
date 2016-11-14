using System;
using System.Linq;
using OpenQA.Selenium;
using Sravni.Common.UI.Page.Common;

namespace Sravni.Common.UI.Page.Bank
{
    public class FilterTabComponent : CommonComponent
    {
        public void ClickFilterTab(FilterTab filterTab)
        {
            var tabName = ConvertTabEnumToString(filterTab);

            var tabElement = Find.Elements(
                By.CssSelector("form.results-container-header > ul.filters-container li > span span"))
                .FirstOrDefault(item => item.Text == tabName);

            Scroll().ScrollToElement(tabElement);
            tabElement.Click();

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