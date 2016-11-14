using System;
using System.Collections.Generic;
using Kepler.Integration.Common.UI.Page.Common;
using Kepler.Integration.Common.UI.Validator;
using OpenQA.Selenium;

namespace Kepler.Integration.Common.UI.Page.Insurance
{
    public class InsuranceCompaniesPage : CommonPage
    {
        public enum SortButton
        {
            Name,
            FoundationYear,
            UserRating
        }

        public void ClickSortButtonInTable(SortButton sortField)
        {
            switch (sortField)
            {
                case SortButton.Name:
                    Find.Element(By.CssSelector("a[href*='propertyName=Name']")).Click();
                    break;
                case SortButton.FoundationYear:
                    Find.Element(By.CssSelector("a[href*='propertyName=FoundationYear']")).Click();
                    break;
                case SortButton.UserRating:
                    Find.Element(By.CssSelector("a[href*='propertyName=UserRating']")).Click();
                    break;

                default:
                    throw new NotImplementedException("Sort field isn't implemented");
            }
        }

        public IEnumerable<IWebElement> GetCompaniesNamesFromTable()
        {
            return Find.Elements(By.CssSelector("div.b-orgscolored-table-td-img > a > img"));
        }


        public InsuranceCompaniesPageValidator GetPageValidator()
        {
            return new InsuranceCompaniesPageValidator(this);
        }
    }
}