using System.Collections.Generic;
using System.Linq;
using Kepler.Integration.Common.UI.Page.Common;
using OpenQA.Selenium;

namespace Kepler.Integration.Common.UI.Page.Rating
{
    public class RatingPage : CommonPage
    {
        protected string ratingItemsSelector = "table.ratings-table tbody > tr";

        public List<IWebElement> GetRatingItems()
        {
            return Find.Elements(By.CssSelector(ratingItemsSelector)).ToList();
        }

        public bool IsRatingItemInViewPort(string ratingItemName)
        {
            var itemSelector = By.CssSelector(ratingItemsSelector + " td.cell-bank-name div.bank-name-inner");
            WaitUntilElementInvisible(itemSelector);
            var element = Find.Element(itemSelector);

            var windowHeight = GetDriver().Manage().Window.Size.Height;

            return element.Displayed && (element.Location.Y < windowHeight);
        }

        public class RatingItem
        {
            public string Title { get; set; }
            public IWebElement Element { get; set; }
        }
    }
}