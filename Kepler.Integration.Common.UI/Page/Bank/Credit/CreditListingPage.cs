using System;
using System.Globalization;
using Kepler.Integration.Common.UI.Page.Bank.Product;
using Kepler.Integration.Common.UI.Validator.BankPageValidator.Product;

namespace Kepler.Integration.Common.UI.Page.Bank.Credit
{
    public class CreditListingPage : BankListingPage<PropositionItem>
    {
        public Tuple<long, long> GetAmountRangeForProposition(string amountRangeField)
        {
            var returnRangeField = new Tuple<long, long>(0, long.MaxValue);

            if (amountRangeField.Contains("не ограничена"))
                return returnRangeField;

            var toIndex = amountRangeField.Length;
            if (amountRangeField.Contains("до"))
            {
                toIndex = amountRangeField.IndexOf("до");

                var number = amountRangeField.Substring(toIndex + 2).Replace(" ", "");
                returnRangeField = new Tuple<long, long>(returnRangeField.Item1, long.Parse(number, CultureInfo.InvariantCulture));
            }

            if (amountRangeField.Contains("от"))
            {
                var fromIndex = amountRangeField.IndexOf("от") + 2;

                var number = amountRangeField.Substring(fromIndex, toIndex - fromIndex).Replace(" ", "");
                returnRangeField = new Tuple<long, long>(long.Parse(number, CultureInfo.InvariantCulture), returnRangeField.Item2);
            }

            return returnRangeField;
        }


        public Tuple<long, long> GetPeriodRangeForProposition(string periodRangeField)
        {
            var returnRangeField = new Tuple<long, long>(0, long.MaxValue);

            var toIndex = periodRangeField.Length;
            if (periodRangeField.Contains("до"))
            {
                toIndex = periodRangeField.IndexOf("до");

                var maxDays = ConvertStringPeriodInDays(periodRangeField.Substring(toIndex + 2));
                returnRangeField = new Tuple<long, long>(returnRangeField.Item1, maxDays);
            }

            if (periodRangeField.Contains("от"))
            {
                var fromIndex = periodRangeField.IndexOf("от") + 2;
                var endPeriodRange = periodRangeField.Substring(fromIndex, toIndex - fromIndex);

                long minDays = 0;
                if (long.TryParse(endPeriodRange, out minDays))
                {
                    minDays *= 365;
                }
                else
                {
                    minDays = ConvertStringPeriodInDays(endPeriodRange);
                }

                returnRangeField = new Tuple<long, long>(minDays, returnRangeField.Item2);
            }

            return returnRangeField;
        }

        public long ConvertStringPeriodInDays(string periodRange)
        {
            var tokens = new[] {"месяц", "год", "лет"};

            long days = 0;

            foreach (var token in tokens)
            {
                var splitResults = periodRange.Split(new[] {token}, StringSplitOptions.RemoveEmptyEntries);

                var daysCoefficient = 0;
                if (token.StartsWith("месяц"))
                {
                    daysCoefficient = 30;
                }
                else
                {
                    daysCoefficient = 365;
                }

                long convertResult = 0;
                foreach (var splitResult in splitResults)
                {
                    if (long.TryParse(splitResult.Trim(), out convertResult))
                        days += daysCoefficient*convertResult;
                }
            }

            return days;
        }


        public new CreditListingPageValidator GetPageValidator()
        {
            return new CreditListingPageValidator(this);
        }
    }
}