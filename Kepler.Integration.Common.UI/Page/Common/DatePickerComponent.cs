using System.Linq;
using System.Threading;
using OpenQA.Selenium;

namespace Kepler.Integration.Common.UI.Page.Common
{
    public class DatePickerComponent : CommonComponent
    {
        public IWebElement DateField { get; set; }
        public By DatePickerSelector { get; set; }
        private bool IsDatePickerInvoked { get; set; }

        public DatePickerComponent Init(By datePickerSelector, By dateFieldSelector)
        {
            DateField = Find.OptionalElement(dateFieldSelector);
            DatePickerSelector = datePickerSelector;

            return this;
        }

        public DatePickerComponent SelectDayInNextMonth(int countNextMonth, int dayNumber)
        {
            InvokeDatePicker();

            for (int index = 0; index < countNextMonth; index++)
            {
                // Because each time when month is changed, datapicker restructure its own dom
                Find.OptionalElement(DatePickerSelector)
                    .FindElement(By.CssSelector("a.ui-datepicker-next"))
                    .Click();

                Thread.Sleep(200);
            }

            SelectDay(dayNumber);

            return this;
        }

        public DatePickerComponent SelectDay(int dayNumber)
        {
            InvokeDatePicker();

            var nextMonthDays = Find.Elements(TestStack.Seleno.PageObjects.Locators.By.jQuery("a.ui-state-default:not(a.ui-priority-secondary)"));
            nextMonthDays.ToList()[dayNumber - 1].Click();

            return this;
        }

        public void InvokeDatePicker()
        {
            if (!IsDatePickerInvoked)
            {
                DateField.Click();
                IsDatePickerInvoked = true;
            }
        }
    }
}