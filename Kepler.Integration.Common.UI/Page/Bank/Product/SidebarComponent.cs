using System.Linq;
using Kepler.Integration.Common.UI.Page.Common;
using OpenQA.Selenium;

namespace Kepler.Integration.Common.UI.Page.Bank.Product
{
    public class SidebarComponent : CommonComponent
    {
        private static string BaseSidebarSelector = "div.l-sidebar ";

        public ProductInfoComponent ProductInfoBlock
        {
            get { return GetComponent<ProductInfoComponent>(); }
        }

        public AdviseComponent AdviseBlock
        {
            get { return GetComponent<AdviseComponent>(); }
        }

        public RatingComponent RatingBlock
        {
            get { return GetComponent<RatingComponent>(); }
        }

        public class ProductInfoComponent : CommonComponent
        {
            public void Init()
            {
                Element = Find.Element(By.CssSelector(BaseSidebarSelector + "a.serp-product-info"));
                Scroll().ScrollToElement(Element);
            }

            public CommonPage Click()
            {
                Init();
                Element.Click();
                return GetComponent<CommonPage>();
            }
        }

        public class AdviseComponent : CommonComponent
        {
            public virtual void Init()
            {
                Element = Find.Elements(By.CssSelector(BaseSidebarSelector + "div.serp-advices")).FirstOrDefault();
                Scroll().ScrollToElement(Element);
            }

            public CommonPage ClickHeader()
            {
                Init();
                Element.FindElement(By.CssSelector("div.heading-small > a")).Click();

                return GetComponent<CommonPage>();
            }

            public CommonPage ClickLinkInBlock(string url)
            {
                Init();
                Element.FindElements(By.CssSelector("div.serp-advices ul > li > a"))
                    .FirstOrDefault(_ => _.GetAttribute("href").EndsWith(url))
                    .Click();

                return GetComponent<CommonPage>();
            }
        }

        public class RatingComponent : AdviseComponent
        {
            public override void Init()
            {
                Element = Find.Elements(By.CssSelector(BaseSidebarSelector + "div.serp-advices")).Last();
                Scroll().ScrollToElement(Element);
            }
        }
    }
}