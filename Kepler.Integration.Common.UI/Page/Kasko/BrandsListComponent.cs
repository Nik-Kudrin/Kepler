using Kepler.Integration.Common.UI.Page.Common;
using OpenQA.Selenium;

namespace Kepler.Integration.Common.UI.Page.Kasko
{
    public class BrandsListComponent : CommonComponent
    {
        private const string BrandsListSelector = ".car-brand-control-inner";


        public void ClickByBrand(string brandName)
        {
            var brandslist = Find.Element(By.CssSelector(BrandsListSelector));

            var selectorForBrandByName = "div > ul > li[data-alias='{0}']";
            var selector = By.CssSelector(string.Format(selectorForBrandByName, brandName));

//            Scroll().ScrollToElement(selector);
            brandslist.FindElement(selector).Click();
        }

        public void ClickShowAllBrands()
        {
            var selector = By.CssSelector(".anchor-block-text");

            Scroll().ScrollToElement(selector);
            Find.Element(selector).Click();
        }

        public int GetCountOfElementsInBrandsList()
        {
            var count = 0;

            var brandslist = Find.Element(By.CssSelector(BrandsListSelector));

            var columns = brandslist.FindElements(By.CssSelector("div.l-col"));
            foreach (var column in columns)
            {
                var blocks = column.FindElements(By.CssSelector(".car-brands-list-by-letter"));
                foreach (var block in blocks)
                {
                    var countElementsInblock = block.FindElements(By.CssSelector("li")).Count - 1;
                    count = count + countElementsInblock;
                }
            }
            return count;
        }
    }
}