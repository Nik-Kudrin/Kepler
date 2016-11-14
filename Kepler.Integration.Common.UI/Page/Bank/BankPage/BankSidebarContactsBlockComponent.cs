using System;
using System.Linq;
using Kepler.Integration.Common.UI.Page.Common;
using OpenQA.Selenium;

namespace Kepler.Integration.Common.UI.Page.Bank.BankPage
{
    public class BankSidebarContactsBlockComponent : CommonComponent
    {
        private string _contactBlockSelector = " div.sidebar-content-container";
        private string _showPhoneBtnSelector = " ul > li.phone-line div.show-button";

        private void InitSidebarBlock()
        {
            // Contacts block is second
            Element = Find.Elements(By.CssSelector(BankPage.SidebarBaseSelector + _contactBlockSelector)).FirstOrDefault();
        }

        public string BankSiteLink
        {
            get
            {
                InitSidebarBlock();
                return Element.FindElement(By.CssSelector("ul > li.website-line span a")).Text;
            }
        }

        public string BankPhone
        {
            get
            {
                InitSidebarBlock();
                return Element.FindElement(By.CssSelector("ul > li.phone-line span")).Text;
            }
        }

        public void ClickShowNumberButton()
        {
            InitSidebarBlock();
            GetShowPhoneButton().Click();
        }

        private IWebElement GetShowPhoneButton()
        {
            return Element.FindElement(By.CssSelector(_showPhoneBtnSelector));
        }

        public bool IsShowPhoneButtonDisplayed()
        {
            try
            {
                GetShowPhoneButton();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}