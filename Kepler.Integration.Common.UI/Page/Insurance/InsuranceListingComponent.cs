using System;
using System.Collections.Generic;
using System.Linq;
using OpenQA.Selenium;
using TestStack.Seleno.PageObjects;
using By = TestStack.Seleno.PageObjects.Locators.By;

namespace Kepler.Integration.Common.UI.Page.Insurance
{
    public class InsuranceListingComponent : UiComponent
    {
        protected IEnumerable<IWebElement> _insuranceResults;

        protected By.jQueryBy OrderPolicyButtonsSelector(int resultItemIndex = 1)
        {
//            return By.CssSelector("div.results__item div.results__item__buttons > a.results__item__buttons__button");
            var x = "div.results__item:first div.results__item__buttons > a.results__item__buttons__button";
            return By.jQuery(x);
        }

        public void CollectStandardResults()
        {
            _insuranceResults = Find.Elements(OpenQA.Selenium.By.CssSelector("div.i-results-layout > div"));
        }

        public bool IsListingHasInsuranceCompany(string insuranceName)
        {
            foreach (var element in _insuranceResults)
            {
                var itemResult = GetComponent<InsuranceResultComponent>();
                itemResult.Element = element;

                if (itemResult.GetItemTitle() == insuranceName)
                    return true;
            }

            return false;
        }


        public IEnumerable<InsuranceResultComponent> FindResultItemByInsuranceName(string insuranceName)
        {
            var resultList = new List<InsuranceResultComponent>();

            foreach (var element in _insuranceResults)
            {
                var itemResult = GetComponent<InsuranceResultComponent>();
                itemResult.Element = element;

                if (itemResult.GetItemTitle() == insuranceName)
                    resultList.Add(itemResult);
            }

            return resultList;
        }

        public IEnumerable<IWebElement> GetStandardResults()
        {
            if (_insuranceResults == null)
            {
                CollectStandardResults();
            }

            return _insuranceResults;
        }

        public IWebElement GetResultItem(int indexInList = 1)
        {
            if (indexInList <= 0)
                throw new Exception("Specify not 0 index");

            if (_insuranceResults == null)
            {
                CollectStandardResults();
            }

            return _insuranceResults.ToArray()[indexInList];
        }

        public bool IsStandardResultsAreAvailable()
        {
            try
            {
                Find.Element(OrderPolicyButtonsSelector(), TimeSpan.FromSeconds(10));
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }

        private bool IsResultsAreAvailable(string resultsBlockName)
        {
            try
            {
                var blockTitles = Find.Elements(OpenQA.Selenium.By.CssSelector("div.results__title > span"), TimeSpan.FromSeconds(10));

                if (blockTitles.Where(x => x.Text == resultsBlockName).Count() > 0)
                    return true;
            }
            catch (Exception ex)
            {
            }

            return false;
        }

        public bool IsSpecialOfferResultsAreAvailable()
        {
            return IsResultsAreAvailable("Специальные предложения");
        }

        public bool IsPopularResultsAreAvailable()
        {
            return IsResultsAreAvailable("Популярный выбор");
        }

        public InsuranceOrderPage ClickOrderPolicyButton(int resultItemIndex = 1)
        {
            return Navigate.To<InsuranceOrderPage>(OrderPolicyButtonsSelector(resultItemIndex));
        }
    }
}