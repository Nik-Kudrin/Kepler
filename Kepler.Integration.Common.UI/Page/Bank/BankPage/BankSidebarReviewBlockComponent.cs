using Kepler.Integration.Common.UI.Page.Common;
using OpenQA.Selenium;

namespace Kepler.Integration.Common.UI.Page.Bank.BankPage
{
    public class BankSidebarReviewBlockComponent : CommonComponent
    {
        private string _reviewBlockSelector = " div.sidebar-add-review";

        private void InitSidebarBlock()
        {
            // Review block is second
            Element = Find.Element(By.CssSelector(BankPage.SidebarBaseSelector + _reviewBlockSelector));
        }

        public CommonPage ClickCreateReview()
        {
            InitSidebarBlock();
            Element.FindElement(By.CssSelector("a")).Click();
            return GetComponent<CommonPage>();
        }
    }
}