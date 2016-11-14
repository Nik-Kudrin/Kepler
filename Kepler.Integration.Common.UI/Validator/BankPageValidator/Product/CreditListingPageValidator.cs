using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Kepler.Integration.Common.UI.Page.Bank.Credit;
using Kepler.Integration.Common.UI.Page.Bank.Product;

namespace Kepler.Integration.Common.UI.Validator.BankPageValidator.Product
{
    public class CreditListingPageValidator : BankPropositionListingPageValidator<CreditListingPage, PropositionItem>
    {
        public CreditListingPageValidator(CreditListingPage page)
            : base(page)
        {
        }


        public void ValidatePropositionsHaveCorrectSummRange(long expectedAmount,
            IEnumerable<BankPropositionComponent<PropositionItem>> propositions)
        {
            foreach (var proposition in propositions)
            {
                var topItem = proposition.GetTopItemResult();
                topItem.ExpandResultItem();

                var amountRangeField = topItem.GetListOfProductFeatures()
                    .FirstOrDefault(item => item.StartsWith("сумма"));

                var range = Page.GetAmountRangeForProposition(amountRangeField);
                range.Item1.Should()
                    .BeLessOrEqualTo(expectedAmount, "Начальная сумма предложения в выдаче должна быть <= заданного условия фильтрации");
                range.Item2.Should().BeGreaterOrEqualTo(expectedAmount, "Макс. сумма предложения в выдаче должна быть >= заданного условия фильтра");
            }
        }


        public void ValidatePropositionsHaveCorrectPeriodRange(string expectedPeriod,
            IEnumerable<BankPropositionComponent<PropositionItem>> propositions)
        {
            long expectedPeriodInDays = 0;
            switch (expectedPeriod.ToLowerInvariant())
            {
                case "полгода":
                    expectedPeriodInDays = Page.ConvertStringPeriodInDays("6 месяцев");
                    break;
                case "год":
                    expectedPeriodInDays = Page.ConvertStringPeriodInDays("1 год");
                    break;
                case "1,5 года":
                    expectedPeriodInDays = Page.ConvertStringPeriodInDays("18 месяцев");
                    break;

                default:
                    expectedPeriodInDays = Page.ConvertStringPeriodInDays(expectedPeriod.ToLowerInvariant());
                    break;
            }


            foreach (var proposition in propositions)
            {
                var topItem = proposition.GetTopItemResult();
                topItem.ExpandResultItem();

                var periodRangeField = topItem.GetListOfProductFeatures()
                    .FirstOrDefault(item => item.StartsWith("срок"));

                var range = Page.GetPeriodRangeForProposition(periodRangeField);

                range.Item1.Should()
                    .BeLessOrEqualTo(expectedPeriodInDays, "Начальный период предложения в выдаче должен быть <= заданного условия фильтрации");
                range.Item2.Should()
                    .BeGreaterOrEqualTo(expectedPeriodInDays, "Макс. период предложения в выдаче должен быть >= заданного условия фильтра");
            }
        }
    }
}