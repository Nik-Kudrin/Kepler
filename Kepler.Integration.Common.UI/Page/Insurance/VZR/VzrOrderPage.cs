using System;
using System.Linq;
using Kepler.Integration.Common.UI.Page.Common;
using Kepler.Integration.Common.UI.Page.Insurance.VZR.Companies;
using OpenQA.Selenium;

namespace Kepler.Integration.Common.UI.Page.Insurance.VZR
{
    public class VzrOrderPage : CommonPage
    {
        public string PolicyPrice { get; private set; }

        public VzrOrderPage FillOrderData(string birthDate, string testEmail)
        {
            FillTravelersData(birthDate);
            FillInsurantData(testEmail);
            return this;
        }

        private void FillTravelersData(string birthDateStr)
        {
            var travelersBlock = GetTravelerBlock();
            var inputFields = travelersBlock.FindElements(By.CssSelector("input.travelers-form__field__input"));

            var passportSeries = inputFields.FirstOrDefault();
            var passportNumber = inputFields[1];
            var secondName = inputFields[2];
            var name = inputFields[3];
            var birthDate = inputFields[4];

            passportSeries.SendKeys("99");
            passportNumber.SendKeys("999999");
            secondName.SendKeys("Testov");
            name.SendKeys("Test");

            birthDate.SendKeys(Keys.End);
            birthDate.SendKeys(Keys.Control + "a");
            birthDate.SendKeys(Keys.Delete);

            birthDate.SendKeys(birthDateStr);
        }

        private void FillInsurantData(string testEmail)
        {
            var insurantBlock = GetInsurantBlock();
            var inputFields = insurantBlock.FindElements(By.CssSelector("input.travelers-form__field__input"));

            var email = inputFields[5];
            var emailVerification = inputFields[6];
            var phone = inputFields[7];

            email.SendKeys(testEmail);
            emailVerification.SendKeys(testEmail);

            phone.SendKeys(Keys.End);
            phone.SendKeys(Keys.Control + "a");
            phone.SendKeys(Keys.Delete);
            phone.SendKeys("999 999-99-99");
        }

        private IWebElement GetTravelerBlock()
        {
            return Find.Elements(By.CssSelector("table.travelers-form")).FirstOrDefault();
        }

        private IWebElement GetInsurantBlock()
        {
            return Find.Elements(By.CssSelector("table.travelers-form")).Last();
        }


        public T ClickBuyPolicy<T>(string insuranceCompanyName) where T : BaseInsurancePage, new()
        {
            var recalcSelector = By.LinkText("Пересчитать");

            if (IsElementExistOnPage(recalcSelector, TimeSpan.FromSeconds(10)))
            {
                var recalc = Find.Element(recalcSelector);

                Scroll().ScrollToElement(recalc);
                recalc.Click();
                WaitUntilOverlay();
            }

            PolicyPrice = Find.Element(By.CssSelector("table.order-form__summary__table tr.m-highlight > td"))
                .Text.Trim().Replace("руб.", "").Replace(" ", "");
            LOG.Info("Policy price " + PolicyPrice);

            var buySelector = By.LinkText("Купить полис");
            var buyElement = Find.Element(buySelector);
            Scroll().ScrollToElement(buyElement);

            return Navigate.To<T>(buySelector);
        }
    }
}