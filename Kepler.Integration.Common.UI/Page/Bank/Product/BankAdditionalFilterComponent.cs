using System;
using Castle.Core.Internal;
using Kepler.Integration.Common.UI.Page.Common;
using Kepler.Integration.Common.UI.SelenoExtension;
using OpenQA.Selenium;

namespace Kepler.Integration.Common.UI.Page.Bank.Product
{
    public class BankAdditionalFilterComponent : CommonComponent, IBankProductFilter
    {
        protected By AllFilterSelector;

        public virtual void SelectAdditionalFilterOption(string optionType, string optionTitle, string optionValueToSelect)
        {
            Find.Element(By.CssSelector("div.details-control-btn")).Click();

            switch (optionType)
            {
                case "checkBox":
                    AllFilterSelector = By.CssSelector("ul.calculator-more-controls-container li.test-control-checkbox");
                    SelectCheckBoxFilterOption(optionTitle);
                    break;
                case "dropDown":
                    if (optionTitle.IsNullOrEmpty())
                        throw new InvalidOperationException("String is null or empty");

                    AllFilterSelector = By.CssSelector("ul.calculator-more-controls-container li.test-control-select");
                    SelectDropDownFilterOption(optionTitle, optionValueToSelect);
                    break;
                default:
                    throw new NotImplementedException();
            }
        }

        protected virtual void SelectCheckBoxFilterOption(string filterOption)
        {
            var optionToCheck = GetDriver()
                .WaitAllElementsAndReturnExpected(AllFilterSelector,
                    item => item.Displayed && item.FindElement(By.CssSelector("input[type='checkbox']")).GetAttribute("name") == filterOption);

            optionToCheck.FindElement(By.CssSelector("span.checkbox-label")).Click();
        }

        protected virtual void SelectDropDownFilterOption(string optionTitle, string optionValueToSelect)
        {
            var optionToCheck = GetDriver()
                .WaitAllElementsAndReturnExpected(AllFilterSelector,
                    item => item.Displayed && item.FindElement(By.CssSelector("label.select-control-label")).Text == optionTitle);

            optionToCheck.FindElement(By.CssSelector("div.select-control-inner--small > div")).Click();

            var elementToSelectInDropDown = GetDriver()
                .WaitAllElementsAndReturnExpected(By.CssSelector("div.ik_select_dropdown li.ik_select_option > span"),
                    element => element.Displayed && element.Text == optionValueToSelect);

            elementToSelectInDropDown.Click();
        }
    }
}