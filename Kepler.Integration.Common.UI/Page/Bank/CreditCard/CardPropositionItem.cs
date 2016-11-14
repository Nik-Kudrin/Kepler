using System;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using Kepler.Integration.Common.UI.Page.Bank.Product;
using Kepler.Integration.Common.UI.SelenoExtension;
using OpenQA.Selenium;

namespace Kepler.Integration.Common.UI.Page.Bank.CreditCard
{
    public class CardPropositionItem : PropositionItem
    {
        protected string FeatureDigitsSelector = "div.result-card-top div.product-info-container div.result-main-features span.digits";
        protected string FeatureNameSelector = "div.result-card-top div.product-info-container div.result-main-features span.dimmed";

        public override string GetProductBankName()
        {
            return
                Element.WaitAndFindElement(
                    By.CssSelector("div.result-card-top div.product-info-container > div.product-info-container-inner div.provider-name"),
                    element => element.Displayed, GetDriver(), TimeSpan.FromSeconds(5))
                    .Text.Trim();
        }

        /// <summary>
        /// Ставка
        /// </summary>
        /// <returns></returns>
        public virtual Tuple<float, float> GetRate()
        {
            var rates = Element.FindElements(By.CssSelector(FeatureDigitsSelector)).FirstOrDefault()
                .Text.Replace("%", "").Replace(",", ".")
                .Split(new[] {"–"}, StringSplitOptions.RemoveEmptyEntries);

            var startRate = float.Parse(rates.First().Trim(), NumberStyles.Any, CultureInfo.InvariantCulture);
            float endRate = 99999999999;

            if (rates.Length == 2)
            {
                endRate = float.Parse(rates[1].Trim(), NumberStyles.Any, CultureInfo.InvariantCulture);
            }

            return new Tuple<float, float>(startRate, endRate);
        }

        /// <summary>
        /// Льготный период
        /// </summary>
        /// <returns></returns>
        public int GetPrivelegesPeriod()
        {
            var period = Element.FindElements(By.CssSelector(FeatureDigitsSelector))[2]
                .Text.Replace("до", "").Replace("дней", "").Trim();

            return int.Parse(period);
        }

        /// <summary>
        /// Кредитный лимит
        /// </summary>
        /// <returns></returns>
        public Tuple<float, float> GetLimits()
        {
            var limits = Element.FindElements(
                By.CssSelector("div.result-card-top div.product-info-container div.result-main-features div.result-card-item"))
                .Where(_ => _.FindElement(By.CssSelector("span.dimmed")).Text.Trim() == "кредитный лимит")
                .Select(_ => _.FindElement(By.CssSelector("span.digits")).Text)
                .FirstOrDefault().Replace(" ", "").Trim().Replace("от", " ").Replace("до", " ")
                .Split(new[] {" "}, StringSplitOptions.RemoveEmptyEntries);

            float startLimit = 0.0F;
            if (limits.Count() > 1)
                startLimit = float.Parse(limits.First().Trim(), CultureInfo.InvariantCulture);
            var endLimit = float.Parse(limits.Last().Trim(), CultureInfo.InvariantCulture);

            return new Tuple<float, float>(startLimit, endLimit);
        }

        public Tuple<float, float> GetCashbackPercents()
        {
            var fullCashback = GetFullCashback();
            return new Tuple<float, float>(fullCashback.Item1, fullCashback.Item2);
        }

        /// <summary>
        /// Return cashback: lower - upper percent boundary, cashback year amount
        /// </summary>
        /// <returns></returns>
        public Tuple<float, float, int> GetFullCashback()
        {
            var boundaries = Element.FindElements(
                By.CssSelector("div.result-card-top div.product-info-container div.result-main-features div.result-card-item"))
                .Where(_ => _.FindElement(By.CssSelector("span.dimmed")).Text.Trim() == "Cashback")
                .Select(_ => _.FindElement(By.CssSelector("span.digits")).Text)
                .FirstOrDefault().Replace("%", "").Replace(",", ".").Replace("до", "").Trim()
                .Split(new[] {"–"}, StringSplitOptions.RemoveEmptyEntries);

            float startLimit = 0;
            if (boundaries.Count() > 1)
                startLimit = float.Parse(boundaries.First().Trim(), CultureInfo.InvariantCulture);

            var pattern = @"\(от(.)*";
            var replacement = "";
            var regex = new Regex(pattern);

            var upperPercent = boundaries.Last().Trim();
            var endLimit = float.Parse(regex.Replace(upperPercent, replacement), CultureInfo.InvariantCulture);
            var cashbackGarbage = regex.Match(upperPercent).ToString().Replace(" ", "");

            pattern = @"\d+";
            regex = new Regex(pattern);
            var cashbackYearAmount = int.Parse(regex.Match(cashbackGarbage).ToString());

            return new Tuple<float, float, int>(startLimit, endLimit, cashbackYearAmount);
        }

        /// <summary>
        /// Мили
        /// </summary>
        /// <returns></returns>
        public Tuple<float, float> GetMiles()
        {
            var boundaries = Element.FindElements(
                By.CssSelector("div.result-card-top div.product-info-container div.result-main-features div.result-card-item"))
                .FirstOrDefault(_ => _.FindElement(By.CssSelector("span.digits")).Text.Trim().Contains("миль"))
                .Text.Trim();

            const string pattern = "миль(.)*";
            const string replacement = "";
            var regex = new Regex(pattern);

            boundaries = regex.Replace(boundaries, replacement);
            var limits = boundaries.Replace(" ", "").Replace("от", " ").Replace("до", " ")
                .Split(new[] {" "}, StringSplitOptions.RemoveEmptyEntries);

            var startLimit = 0.0F;
            if (boundaries.Count() > 1)
                startLimit = float.Parse(limits.First().Trim(), CultureInfo.InvariantCulture);
            var endLimit = float.Parse(limits.Last().Trim(), CultureInfo.InvariantCulture);

            return new Tuple<float, float>(startLimit, endLimit);
        }

        public bool IsPropositionFeatureExist(string featureName)
        {
            return Element.FindElements(By.CssSelector(FeatureNameSelector)).Any(_ => _.Text.Trim() == featureName);
        }

        public bool IsAnyPropositionFeatureContainsCurrencySymbol(string currencySymbol)
        {
            var features = Element.FindElements(By.CssSelector(FeatureDigitsSelector))
                .Select(_ => _.Text.Trim()).ToList();
            features.AddRange(GetListOfProductFeatures());

            return features.Any(_ => _.Contains(currencySymbol));
        }

        public override bool CheckIsAdCard()
        {
            try
            {
                var element = Element.WaitAndFindElement(By.CssSelector("div.result-card-btn-container > a"),
                    _ => _.Displayed, GetDriver(), TimeSpan.FromSeconds(5));
                if (element == null)
                    return false;
            }
            catch (Exception ex)
            {
                return false;
            }

            return true;
        }

        public void ClickProductLogo()
        {
            Scroll().ScrollToElement(Element);
            Element.FindElement(By.CssSelector("div.product-pic")).Click();
        }
    }
}