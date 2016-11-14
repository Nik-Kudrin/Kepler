using System;
using System.Globalization;
using System.Linq;
using Kepler.Integration.Common.UI.Page.Bank.CreditCard;
using OpenQA.Selenium;

namespace Kepler.Integration.Common.UI.Page.Bank.DebitCard
{
    public class DebitCardPropositionItem : CardPropositionItem
    {
        /// <summary>
        /// Процентная ставка
        /// </summary>
        /// <returns></returns>
        public override Tuple<float, float> GetRate()
        {
            var rates = Element.FindElements(By.CssSelector(FeatureDigitsSelector)).FirstOrDefault()
                .Text.Replace("%", "").Replace(",", ".").Replace("до", "–")
                .Split(new[] {"–"}, StringSplitOptions.RemoveEmptyEntries);

            var startRate = 0;
            float endRate = float.Parse(rates.First().Trim(), NumberStyles.Any, CultureInfo.InvariantCulture);

            if (rates.Length == 2)
            {
                endRate = float.Parse(rates[1].Trim(), NumberStyles.Any, CultureInfo.InvariantCulture);
            }

            return new Tuple<float, float>(startRate, endRate);
        }
    }
}