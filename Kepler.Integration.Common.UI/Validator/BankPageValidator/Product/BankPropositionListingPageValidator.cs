using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Kepler.Integration.Common.UI.Page.Bank.Product;
using TestStack.Seleno.Extensions;
using TestStack.Seleno.PageObjects.Locators;

namespace Kepler.Integration.Common.UI.Validator.BankPageValidator.Product
{
    public class BankPropositionListingPageValidator<T, M> : BasePageValidator<T>
        where T : BankListingPage<M>
        where M : PropositionItem, new()
    {
        public BankPropositionListingPageValidator(T page) : base(page)
        {
        }

        protected BankPropositionListingPageValidator() : base(null)
        {
        }

        public enum SortingOrder
        {
            Ascending,
            Descending
        }

        public SortingOrder ConvertSortOrderStringToEnum(string sortOrder)
        {
            switch (sortOrder)
            {
                case "ascendingSorting":
                    return SortingOrder.Ascending;
                case "descendingSorting":
                    return SortingOrder.Descending;
                default:
                    throw new NotImplementedException(sortOrder + " sorting order isn't implemented");
            }
        }

        public virtual void ValidateSorting(string sortingType, string stringSortOrder)
        {
            var sortingOrder = ConvertSortOrderStringToEnum(stringSortOrder);

            switch (sortingType)
            {
                case "по доходности":
                    ValidateSortingIsCorrect(item => item.GetTopItemResult().GetIncomeInDigit(), sortingOrder);
                    break;
                case "по процентной ставке":
                case "процентная ставка":
                    ValidateSortingIsCorrect(item => item.GetTopItemResult().GetRate(), sortingOrder);
                    break;
                case "по оценке посетителей":
                    ValidateSortingIsCorrect(item => Convert.ToInt32(item.GetUserRate()), sortingOrder);
                    break;
                case "по названию банка":
                case "название банка":
                    ValidateSortingIsCorrect(item => item.GetTopItemResult().GetProductBankName(), sortingOrder);
                    break;
                default:
                    throw new NotImplementedException(sortingType + " sorting type isn't implemented");
            }
        }

        protected StandardPropositionListingComponent<M> GetStandartPropositionComponent()
        {
            return Page.GetStandardPropositions<StandardPropositionListingComponent<M>>();
        }

        protected SpecialPropositionListingComponent<M> GetSpecialPropositionComponent()
        {
            return Page.GetSpecialPropositions<SpecialPropositionListingComponent<M>>();
        }


        protected virtual void ValidateSortingIsCorrect<T>(Func<BankPropositionComponent<M>, T> predicate,
            SortingOrder sortOrder = SortingOrder.Descending)
        {
            var sourceList = GetStandartPropositionComponent().GetPropositionsResult()
                .Select(predicate).ToList();
            var sortedList = SortList(sourceList, sortOrder);

            ValidateListSequenceIsEqual(sourceList, sortedList);
        }


        public void ValidateSortingInGroup(BankPropositionComponent<M> resultComponent, string sortingType,
            string stringSortOrder)
        {
            var sortOrder = ConvertSortOrderStringToEnum(stringSortOrder);

            switch (sortingType)
            {
                case "по доходности":
                    ValidateSortingInGroupIsCorrect(resultComponent, item => item.GetIncomeInDigit(), sortOrder);
                    break;
                case "по процентной ставке":
                case "процентная ставка":
                    ValidateSortingInGroupIsCorrect(resultComponent, item => item.GetRate(), sortOrder);
                    break;
                default:
                    throw new NotImplementedException();
            }
        }

        protected void ValidateSortingInGroupIsCorrect<T>(BankPropositionComponent<M> resultComponent,
            Func<PropositionItem, T> predicate, SortingOrder sortOrder = SortingOrder.Descending)
        {
            var sourceList = resultComponent.GetResultGroup()
                .Select(predicate).ToList();
            var sortedList = SortList(sourceList, sortOrder);

            ValidateListSequenceIsEqual(sourceList, sortedList);
        }

        protected virtual List<T> SortList<T>(List<T> sourceList, SortingOrder sortOrder)
        {
            List<T> sortedList = null;

            switch (sortOrder)
            {
                case SortingOrder.Descending:
                    return sourceList.OrderByDescending(item => item).ToList();
                case SortingOrder.Ascending:
                    return sourceList.OrderBy(item => item).ToList();
                default:
                    throw new NotImplementedException();
            }
        }


        protected void ValidateListSequenceIsEqual<T>(List<T> sourceList, List<T> sortedList)
        {
            LOG.Info("Валидация сортировки ...");

            var errorMessage =
                string.Format(
                    "Выдача должна быть корректно отсортирована. Различия в списках: >>> 'Expected': {0} ; >>> 'Actual': {1}",
                    string.Join("', '", sortedList), string.Join("', '", sourceList));

            sourceList.SequenceEqual(sortedList).Should().BeTrue(errorMessage);
        }


        public void ValidatePeriodFilteringIsCorrectlyApplied(string period)
        {
            Page.WaitUntilListingOverlayIsDisplayed();

            switch (period)
            {
                case "3 месяца":
                    ValidatePeriodDays(90);
                    break;
                case "Год":
                    ValidatePeriodDays(359);
                    break;
                case "2 года":
                    ValidatePeriodDays(700);
                    break;

                default:
                    throw new NotImplementedException();
            }
        }

        protected void ValidatePeriodDays(int periodDays)
        {
            GetSpecialPropositionComponent().GetPropositionsResult().
                Each(element => element.GetTopItemResult().GetPeriodInDays().Should().BeGreaterOrEqualTo(periodDays));

            GetStandartPropositionComponent().GetPropositionsResult().
                Each(element => element.GetTopItemResult().GetPeriodInDays().Should().BeGreaterOrEqualTo(periodDays));
        }

        public virtual void ValidateCurrencySortingIsCorrectlyApplied(string currencySymbol)
        {
            Page.WaitUntilListingOverlayIsDisplayed();

            switch (currencySymbol)
            {
                case "$":
                case "€":
                case "евро":
                case "доллары":
                    ValidateCurrencyInListing(currencySymbol);
                    break;
                case "Р":
                case "рубли":
                    ValidateRoubleCurrencyInListing();
                    break;

                default:
                    throw new NotImplementedException();
            }
        }

        protected virtual void ValidateCurrencyInListing(string currencySymbol)
        {
            GetStandartPropositionComponent().GetPropositionsResult().Take(6)
                .Each(item => item.GetTopItemResult().GetIncome()
                    .LastIndexOf(currencySymbol)
                    .ShouldBeEquivalentTo(item.GetTopItemResult().GetIncome().Length - 1,
                        "Символ валюты должен отображаться после суммы дохода"));
        }

        protected virtual void ValidateRoubleCurrencyInListing()
        {
            GetStandartPropositionComponent().GetPropositionsResult().Take(6)
                .Each(item => item.GetTopItemResult()
                    .Element.FindElement(
                        By.CssSelector("span.popup-container span.bank-product-digit-value > span"))
                    .GetAttribute("class")
                    .ShouldBeEquivalentTo("rouble",
                        "Для отображения знака рубля должен навешиваться соответствующий класс"));
        }

        public void ValidateGroupsIsDisplayed(BankPropositionComponent<M> depositResultComponent,
            bool isDisplayed = false)
        {
            depositResultComponent.GetResultGroup()
                .Each(item => item.Element.Displayed
                    .ShouldBeEquivalentTo(isDisplayed,
                        string.Format("В выдаче должны отображаться сгруппированные элементы ='{0}'", isDisplayed)));
        }

        public void ValidateCountPropositons(int expectedCount)
        {
            var actualCount = Page.GetCountPropositionsFromShowMoreButton();
            actualCount.Should().BeGreaterThan(expectedCount, "Количество предложение не увеличилось");
        }
    }
}