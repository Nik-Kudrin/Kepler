using System;
using System.Collections.Generic;
using System.Linq;
using Kepler.Integration.Common.UI.Page.Common;
using OpenQA.Selenium;

namespace Kepler.Integration.Common.UI.Page.Kasko
{
    public class ResultCard : CommonComponent
    {
        IWebElement _resultCardComponent;

        public ResultCard(IWebElement resultWebElem)
        {
            _resultCardComponent = resultWebElem;
        }

        public int GetPrice()
        {
            var selectorStr =
                "div.rcb-product-info-container-top > div.rcb-product-feature-main.rcb-product-feature-main--big > span > span:nth-child(1) > span:nth-child(1)";

            var priceElem = _resultCardComponent.FindElement(By.CssSelector(selectorStr));

            var intPrice = Int32.Parse(priceElem.Text.Replace(" ", string.Empty));

            return intPrice;
        }

        public int GetFranchiseValue()
        {
            const string selectorStr = "div.rcb-product-info-container-top > div:nth-child(4) > span ";

            var franchiseValue = _resultCardComponent.FindElement(By.CssSelector(selectorStr));

            if (franchiseValue.Text.Contains("Без франшизы"))
            {
                return 0; //Значение франшизы равно 0 для карточек без франшизы
            }

            franchiseValue = franchiseValue.FindElement(By.CssSelector("span:nth-child(1)"));
            var intPrice = Int32.Parse(franchiseValue.Text.Replace(" ", string.Empty));

            return intPrice;
        }

        public List<string> GetEnabledAdditionalFilters()
        {
            var allWEbElementsFilters =
                _resultCardComponent.FindElements(By.CssSelector("li.rcb-product-feature.rcb-product-feature--enabled"));

            var allFilters = allWEbElementsFilters.Select(webelem => webelem.Text.ToString()).ToList();

            return allFilters;
        }

        public int GetCustomerRatingValue()
        {
            var selectorStr =
                "div.rcb-company-info-container-top > div.rcb-company-rating-reviews.popup-container > div > span:nth-child(1)";

            var customerRatingValueElem = _resultCardComponent.FindElement(By.CssSelector(selectorStr));

            return Int32.Parse(customerRatingValueElem.Text);
        }

        public int GetRatingValueInt()
        {
            var selectorStr = "div.rcb-company-info-container-bottom";

            var ratingValuElement = _resultCardComponent.FindElement(By.CssSelector(selectorStr));

            if (!ratingValuElement.Text.Contains("Рейтинг")) return 0;
            var raitingValue = ratingValuElement.Text.Replace("Рейтинг ", String.Empty);

            if ((new[] {"B--", "B-", "B", "B+", "В--", "В-", "В", "В+"}).Contains(raitingValue))
                return 1;
            if ((new[] {"B++", "В++"}).Contains(raitingValue))
                return 2;
            if ((new[] {"A--", "A-", "А--", "А-"}).Contains(raitingValue))
                return 3;
            if ((new[] {"A", "А"}).Contains(raitingValue))
                return 4;
            if ((new[] {"A+", "А+"}).Contains(raitingValue))
                return 5;
            if ((new[] {"A++", "А++"}).Contains(raitingValue))
                return 6;

            return -1;
        }

        public void ClickGetOrder()
        {
            var selectorStr = "div.rcb-btn-container-top > div.btn-container";
            var orderButton = _resultCardComponent.FindElement(By.CssSelector(selectorStr));
            orderButton.Click();
        }
    }
}