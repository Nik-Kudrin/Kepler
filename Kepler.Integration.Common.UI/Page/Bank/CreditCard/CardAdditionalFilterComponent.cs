using System;
using Castle.Core.Internal;
using Kepler.Integration.Common.UI.Page.Bank.Product;
using OpenQA.Selenium;

namespace Kepler.Integration.Common.UI.Page.Bank.CreditCard
{
    public class CardAdditionalFilterComponent : BankAdditionalFilterComponent
    {
        public override void SelectAdditionalFilterOption(string optionType, string optionTitle, string optionValueToSelect)
        {
            Find.Element(By.CssSelector("div.details-control-btn")).Click();

            switch (optionType)
            {
                case "checkBox":
                    AllFilterSelector = By.CssSelector("div.checkbox-control");
                    SelectCheckBoxFilterOption(optionTitle);
                    break;
                case "dropDown":
                    if (optionTitle.IsNullOrEmpty())
                        throw new InvalidOperationException("String is null or empty");

                    AllFilterSelector = By.CssSelector("div.select-control");
                    SelectDropDownFilterOption(optionTitle, optionValueToSelect);
                    break;
                default:
                    throw new NotImplementedException("Filter not implemented for option " + optionType);
            }
        }
    }
}