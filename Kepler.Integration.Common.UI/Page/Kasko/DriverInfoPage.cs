using System;
using System.Linq;
using Kepler.Integration.Common.UI.Page.Common;
using OpenQA.Selenium;

namespace Kepler.Integration.Common.UI.Page.Kasko
{
    public class DriverInfoPage : CommonPage
    {
        private readonly IWebElement _page;

        public DriverInfoPage(IWebElement page)
        {
            _page = page;
        }

        public readonly string[] NumberOfChildrenArray = {"Нет детей", "1", "2", "3", "4 и более"};

        public void SelectRandomSex()
        {
            var rnd = new Random();
            var isMarried = rnd.Next(0, 1);

            var sex = isMarried == 1 ? "Мужской" : "Женский";
            SelectSex(sex);
        }

        public void SelectSex(string sex)
        {
            var drivingExperienceField = _page.FindElement(By.CssSelector("form > div:nth-child(4) > div"));

            var webElements = drivingExperienceField.FindElement(By.CssSelector(".radio-control-inner"))
                .FindElements(By.CssSelector("ul > li > label"));
            var webElement = webElements.FirstOrDefault(element => element.Text.Contains(sex));
            if (webElement != null) webElement.Click();
        }

        public void SelectRandomIsMarried()
        {
            var rnd = new Random();
            var isMarried = rnd.Next(0, 1) == 1;
            SelectIsMarried(isMarried);
        }

        public void SelectIsMarried(bool isMarried)
        {
            const string yes = "Да";
            const string no = "Не";

            var sex = isMarried ? yes : no;

            var marriedField = _page.FindElement(By.CssSelector("form > div:nth-child(5) > div"));

            var webElements =
                marriedField.FindElement(By.CssSelector(".radio-control-inner"))
                    .FindElements(By.CssSelector("ul > li > label"));
            var webElement = webElements.FirstOrDefault(element => element.Text.Contains(sex));
            if (webElement != null) webElement.Click();
        }

        public void SelectRandomNumberOfChildren()
        {
            var rnd = new Random();
            var childNumber = rnd.Next(rnd.Next(NumberOfChildrenArray.Length));

            SelectNumberOfChildren(childNumber);
        }

        public void SelectNumberOfChildren(int childNumber)
        {
            var marriedField = _page.FindElement(By.CssSelector("form > div:nth-child(6) > div "));

            var webElements =
                marriedField.FindElement(By.CssSelector(".radio-control-inner"))
                    .FindElements(By.CssSelector("ul > li > label"));
            var webElement =
                webElements.FirstOrDefault(element => element.Text.Contains(NumberOfChildrenArray[childNumber]));
            if (webElement != null) webElement.Click();
        }

        public void SelectValueInRadioControl(string stringValue, string fieldLabel, string radioControlItem)
        {
            var radioControls = _page.FindElements(By.CssSelector("form > div > div.radio-control"));

            var radioControl = radioControls.FirstOrDefault(element => element.Text.Contains(fieldLabel));
            var radioControlItems =
                radioControl.FindElement(By.CssSelector(".radio-control-inner"))
                    .FindElements(By.CssSelector("ul > li > label"));

            var radioControlItemElem =
                radioControlItems.FirstOrDefault(element => element.Text.Contains(radioControlItem));
            if (radioControlItemElem != null) radioControlItemElem.Click();
        }

        public void EnterDriverAge(string driverAge)
        {
            EnterStringInSliderByFieldLabel(driverAge, "Введите возраст (лет)");
        }

        public void EnterDrivingExperience(string experienceByYears)
        {
            EnterStringInSliderByFieldLabel(experienceByYears, "Введите стаж (лет)");
        }

        public void EnterDriverNameWithOutLimits(string driverName)
        {
            EnterStringInTextControlByFieldLabel(driverName, "Как к вам обращаться?");
        }

        public void EnterDriverPhoneNumberWithOutLimits(string driverPhoneNumber)
        {
            EnterStringInTextControlByFieldLabel(driverPhoneNumber, "Телефон для расчета");
        }

        public void EnterStringInSliderByFieldLabel(string stringValue, string fieldLabel)
        {
            var fields =
                _page.FindElements(By.CssSelector("form > div.l-col > div.slider-control"));

            var webElement = fields.FirstOrDefault(element => element.Text.Contains(fieldLabel));
            var webElement2 = webElement.FindElement(By.CssSelector("input"));
            webElement2.Clear();
            webElement2.SendKeys(stringValue);
        }

        public void EnterStringInTextControlByFieldLabel(string stringValue, string fieldLabel)
        {
            var fields =
                _page.FindElements(By.CssSelector("form > div.l-col > div.text-control"));

            var webElement = fields.FirstOrDefault(element => element.Text.Contains(fieldLabel));
            var webElement2 = webElement.FindElement(By.CssSelector("div.text-control-input > input"));
            webElement2.Click();
            webElement2.SendKeys(stringValue);
        }

        public void CheckedCheckBox()
        {
            var checkbox =
                _page.FindElement(
                    By.CssSelector("form > div.l-col.l-col--type-2 > div > label > span.checkbox-label > span"));
            checkbox.Click();
        }

        public void ClickShowResults()
        {
            var selector = By.CssSelector("form > div.l-col.l-col--type-4 > div.anchor-block ");

            _page.FindElement(selector).Click();
        }
    }
}