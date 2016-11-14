using Kepler.Integration.Common.UI.Page.Organization;

namespace Kepler.Integration.Common.UI.Page.Bank.BankPage
{
    public class BankPage : OrganizationPage
    {
        public static string SidebarBaseSelector = "div.l-sidebar--display-block ";

        public BankSidebarContactsBlockComponent GetSidebarContactsBlock
        {
            get { return GetComponent<BankSidebarContactsBlockComponent>(); }
        }

        public BankSidebarReviewBlockComponent GetReviewBlock
        {
            get { return GetComponent<BankSidebarReviewBlockComponent>(); }
        }

        public BankProductsBlockComponent GetProductBlocks
        {
            get { return GetComponent<BankProductsBlockComponent>(); }
        }
    }
}