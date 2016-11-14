using System.Threading;
using Kepler.Integration.Common.UI.Page.Common;
using TestStack.Seleno.PageObjects.Locators;

namespace Kepler.Integration.Common.UI.Page.Insurance.VZR
{
    public class VzrWizardPage : CommonPage
    {
        public VzrWizardPage ClickCountry(string countryName)
        {
            Find.Element(By.CssSelector("a[href=\"/vzr/" + countryName + "/\"]")).Click();
            return this;
        }


        public VzrWizardPage FillStartDate()
        {
            GetComponent<DatePickerComponent>()
                .Init(By.CssSelector("div#ui-datepicker-div"), By.CssSelector("input.i-date-picker-from"))
                .SelectDayInNextMonth(1, 1);

            return this;
        }

        public VzrWizardPage FillEndDate()
        {
            GetComponent<DatePickerComponent>()
                .Init(By.CssSelector("div#ui-datepicker-div"), By.CssSelector("input.i-date-picker-to"))
                .SelectDayInNextMonth(2, 1);

            return this;
        }

        public VzrWizardPage FillBirthDate(string dateString)
        {
            GetComponent<DatePickerComponent>()
                .Init(By.CssSelector("div#ui-datepicker-div"), By.CssSelector("input#birthDate"))
                .SelectDay(15);

            // Just to close Date picker window
            Find.Element(OpenQA.Selenium.By.CssSelector("h1.title__value")).Click();
            Thread.Sleep(1000);

            return this;
        }


        public VzrWizardPage ClickGoToTourist()
        {
            Find.Element(By.CssSelector("button[data-bind=\"click: onMoveToTourists\"]")).Click();
            return this;
        }

        public VzrWizardPage ClickSelectActivity()
        {
            Find.Element(By.CssSelector("button#selectActivity")).Click();
            return this;
        }

        public VzrListingPage ClickShowResults()
        {
            var resultsSelector = By.CssSelector("button[data-bind=\"click: showResults\"]");
            return Navigate.To<VzrListingPage>(resultsSelector);
        }
    }
}