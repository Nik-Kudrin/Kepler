using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using Castle.Core.Internal;
using Kepler.Integration.Common.UI.Page.Common;
using Kepler.Integration.Common.UI.SelenoExtension;
using OpenQA.Selenium;

namespace Kepler.Integration.Common.UI.Page.Bank.Product
{
    public class PropositionItem : CommonComponent
    {
        public IWebElement Element { get; set; }

        public void Init(IWebElement element)
        {
            Element = element;
        }

        public string GetIncome()
        {
            return Element.FindElement(By.CssSelector("span.popup-container span.bank-product-digit-value")).Text;
        }

        public int GetIncomeInDigit()
        {
            var stringIncome = GetIncome();
            stringIncome = stringIncome.Substring(0, stringIncome.Length - 1)
                .Replace("+", "").Replace("-", "").Replace(" ", "").Replace(",", "").Trim();

            return Convert.ToInt32(stringIncome);
        }

        public int GetPeriodInDays()
        {
            var stringPeriod =
                Element.FindElement(By.CssSelector("span.deposit-time-value-container span.deposit-profit-value")).Text;
            stringPeriod = stringPeriod.Substring(0, stringPeriod.IndexOf(" ")); // get rid of "дней"
            return Convert.ToInt32(stringPeriod.Trim());
        }

        public virtual float GetRate()
        {
            var rateString = Element.FindElement(By.CssSelector("span.test-rate-value"))
                .Text.Replace(",", ".").Replace("%", "").Trim();
            return Single.Parse(rateString, CultureInfo.InvariantCulture);
        }

        public void ExpandResultItem()
        {
            var resultClickElement = Element.WaitAndFindElement(By.CssSelector("div.result-card-top"),
                element => element.Displayed, GetDriver());

            Scroll().ScrollToElement(resultClickElement);
            resultClickElement.Click();
        }

        public void ClickLinkProductPageDetails()
        {
            Element.WaitAndFindElement(By.CssSelector("div.result-more-features div.result-card-full-link > a"),
                element => element.Displayed, GetDriver())
                .Click();
        }

        public void ClickLinkButtonMore()
        {
            var elem = Element.WaitAndFindElement(By.CssSelector("div.result-card-btn-container > a"),
                element => element.Displayed, GetDriver());

            Scroll().ScrollToElement(elem);
            elem.Click();
        }

        public void ClickLinkBankLogo()
        {
            var elem = Element.WaitAndFindElement(By.CssSelector("div.company-logo"),
                element => element.Displayed, GetDriver());

            Scroll().ScrollToElement(elem);
            elem.Click();
        }

        public void ClickLinkProductName()
        {
            var elem = Element.WaitAndFindElement(By.CssSelector("div.result-card-top .result-name"),
                element => element.Displayed, GetDriver());

            Scroll().ScrollToElement(elem);
            elem.Click();
        }

        public virtual bool CheckIsAdCard()
        {
            var moreButton = Element.FindElement(By.CssSelector("div.result-card-btn-container > a"));

            var href = moreButton.GetAttribute("href");
            var contain2Cloud = href.Contains("sravni.go2cloud.org");

            if (contain2Cloud == false)
                contain2Cloud = href.Contains("/landing/");

            return contain2Cloud;
        }


        public bool CheckIsAdCardWithoutPopup()
        {
            var moreButton = Element.FindElement(By.CssSelector("div.result-card-btn-container > a"));

            var href = moreButton.GetAttribute("href");
            var contain2Cloud = href.Contains("sravni.go2cloud.org");

            return contain2Cloud;
        }

        /// <summary>
        /// Return only features with non empty text data
        /// </summary>
        /// <returns></returns>
        public IEnumerable<string> GetListOfProductFeatures()
        {
            var featuresSelector = By.CssSelector("div.result-more-features li.result-feature");

            GetDriver().WaitUntilElementInvisible(featuresSelector);

            return Element.FindElements(featuresSelector)
                .Where(item => !item.Text.IsNullOrEmpty() && !item.GetAttribute("class").Contains("result-feature--minus"))
                .Select(item => item.Text.Trim().ToLowerInvariant());
        }

        public virtual string GetProductName()
        {
            var fullProductText = Element.WaitAndFindElement(By.CssSelector("div.result-card-top .result-name"),
                element => element.Displayed, GetDriver(), TimeSpan.FromSeconds(5))
                .Text;

            return fullProductText.Remove(0, fullProductText.IndexOf("«") + 1).Replace("»", "");
        }

        public virtual string GetProductBankName()
        {
            return Element.WaitAndFindElement(By.CssSelector("div.result-card-top div.company-logo img"),
                element => element.Displayed, GetDriver(), TimeSpan.FromSeconds(5))
                .GetAttribute("alt").Trim();
        }

        public string GetPixelUrl()
        {
            string pixelHref;

            try
            {
                var pixelElement = Element.WaitAndFindElement(By.CssSelector("div.result-card-btn-container > a"),
                    element => element.Displayed, GetDriver(), TimeSpan.FromSeconds(2));

                if (pixelElement == null)
                    return null;

                Scroll().ScrollToElement(pixelElement);
                pixelHref = pixelElement.GetAttribute("href");
            }
            catch (Exception ex)
            {
                return null;
            }

            // Decode pixel urls in human readable format
            return HttpUtility.UrlDecode(pixelHref.Substring(pixelHref.IndexOf("=http") + 1));
        }
    }
}