using System;
using Kepler.Integration.Common.UI.Page.Common;
using Kepler.Integration.Common.UI.SelenoExtension;
using TestStack.Seleno.PageObjects.Locators;

namespace Kepler.Integration.Common.UI.Page.Insurance
{
    public class InsuranceListingPage : CommonPage
    {
        public InsuranceListingComponent InsuranceListing
        {
            get { return GetComponent<InsuranceListingComponent>(); }
        }

        public InsuranceListingPage ClickForwardMailButton()
        {
            Find.Element(By.CssSelector("a.i-link-send-letter")).Click();
            return this;
        }

        public InsuranceListingPage TypeEmailInForwardWindow(String email)
        {
            var element = Find.Element(By.CssSelector("div.standart-form__item input"));
            Scroll().ScrollToElement(element);
            element.SendKeys(email);
            return this;
        }

        public InsuranceListingPage ClickSendEmail()
        {
            OpenQA.Selenium.By sendEmailSelector = By.CssSelector("input.i-email-send");

            var button = Browser.WaitAndFindElement(sendEmailSelector, TimeSpan.FromSeconds(15));
            Scroll().ScrollToElement(button);
            button.Click();

            return this;
        }
    }
}