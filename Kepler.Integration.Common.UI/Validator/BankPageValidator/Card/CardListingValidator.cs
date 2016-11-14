using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Kepler.Integration.Common.UI.Page.Bank.CreditCard;
using Kepler.Integration.Common.UI.Page.Bank.Product;
using Kepler.Integration.Common.UI.Validator.BankPageValidator.Product;
using OpenQA.Selenium;

namespace Kepler.Integration.Common.UI.Validator.BankPageValidator.Card
{
    public class CardListingValidator<T, X> : BankPropositionListingPageValidator<BankListingPage<X>, X>, IValidateCard<X>
        where T : CardListingPage
        where X : CardPropositionItem, new()
    {
        protected new T Page { get; set; }

        public CardListingValidator()
        {
        }

        public CardListingValidator(T page) : base(page.GetPage<BankListingPage<X>>())
        {
            Page = page;
        }

        protected virtual M GetStandartPropositionComponent<M, Z>()
            where M : StandardPropositionListingComponent<X>, IResult<Z, X>, new()
            where Z : CardPropositionComponent<X>, new()
        {
            return Page.GetPropositions<M, Z, X>();
        }

        protected virtual M GetSpecialPropositionComponent<M, Z>()
            where M : SpecialPropositionListingComponent<X>, IResult<Z, X>, new()
            where Z : CardPropositionComponent<X>, new()
        {
            return Page.GetPropositions<M, Z, X>();
        }


        public void ValidateSorting<M, Z>(string sortingType, string stringSortOrder)
            where M : StandardPropositionListingComponent<X>, IResult<Z, X>, new()
            where Z : CardPropositionComponent<X>, new()
        {
            var sortingOrder = ConvertSortOrderStringToEnum(stringSortOrder);

            switch (sortingType)
            {
                case "% ставке": // % ставке
                case "процент на остаток":
                    ValidateSortingIsCorrect<M, Z>(item => item.GetTopItemResult().GetRate(), sortingOrder, true);
                    break;
                case "льготному периоду": // льготному периоду
                    ValidateSortingIsCorrect<M, Z, int>(item => item.GetTopItemResult().GetPrivelegesPeriod(), sortingOrder);
                    break;
                case "кредитному лимиту": // кредитному лимиту
                    ValidateSortingIsCorrect<M, Z>(item => item.GetTopItemResult().GetLimits(), sortingOrder, false);
                    break;
                case "CashBack": // CashBack
                    ValidateSortingIsCorrect<M, Z>(item => item.GetTopItemResult().GetCashbackPercents(), sortingOrder, true);
                    break;
                case "количеству миль": // количеству миль
                    ValidateSortingIsCorrect<M, Z>(item => item.GetTopItemResult().GetMiles(), sortingOrder, false);
                    break;
                case "названию банка": // названию банка
                    ValidateSortingIsCorrect<M, Z, string>(item => item.GetTopItemResult().GetProductBankName(), sortingOrder);
                    break;

                default:
                    throw new NotImplementedException(sortingType + " sorting type isn't implemented");
            }
        }

        protected void ValidateSortingIsCorrect<M, Z>(Func<Z, Tuple<float, float>> predicate,
            SortingOrder sortOrder, bool sortByFirstItem)
            where M : StandardPropositionListingComponent<X>, IResult<Z, X>, new()
            where Z : CardPropositionComponent<X>, new()
        {
            // example Tuple<22,1; 23,0> (range of credit card rates)
            var sourceList = (GetStandartPropositionComponent<M, Z>() as IResult<Z, X>).GetPropositionsResult()
                .Select(predicate).ToList();

            sourceList.ForEach(_ => _.Item2.Should()
                .BeGreaterOrEqualTo(_.Item1, "Второе значение для фильтруемого параметра в кредит. картах не должно быть меньше первого"));

            var itemList = new List<float>();

            if (sortByFirstItem)
                itemList = sourceList.Select(item => item.Item1).ToList();
            else
            {
                itemList = sourceList.Select(item => item.Item2).ToList();
            }

            var sortedList = SortList(itemList, sortOrder);

            ValidateListSequenceIsEqual(itemList, sortedList);
        }

        protected void ValidateSortingIsCorrect<M, Z, TT>(Func<Z, TT> predicate,
            SortingOrder sortOrder = SortingOrder.Descending)
            where M : StandardPropositionListingComponent<X>, IResult<Z, X>, new()
            where Z : CardPropositionComponent<X>, new()
        {
            var sourceList = (GetStandartPropositionComponent<M, Z>() as IResult<Z, X>).GetPropositionsResult()
                .Select(predicate).ToList();
            var sortedList = SortList(sourceList, sortOrder);

            ValidateListSequenceIsEqual(sourceList, sortedList);
        }

        protected override void ValidateRoubleCurrencyInListing()
        {
            var propositions = GetStandartPropositionComponent().GetPropositionsResult().Take(4);

            foreach (var propositionItem in propositions)
            {
                var topItemResult = propositionItem.GetTopItemResult();
                topItemResult.ExpandResultItem();

                propositionItem.PropositionItem
                    .Element.FindElements(By.CssSelector("span.rouble"))
                    .Should().NotBeNullOrEmpty("Символ валюты должен отображаться в каком-либо поле инфы по карте");
            }
        }

        protected new void ValidateCurrencyInListing<M, Z>(string currencySymbol)
            where M : StandardPropositionListingComponent<X>, IResult<Z, X>, new()
            where Z : CardPropositionComponent<X>, new()
        {
            var propositions = (GetStandartPropositionComponent<M, Z>() as IResult<Z, X>).GetPropositionsResult().Take(6);

            foreach (var propositionItem in propositions)
            {
                var topItemResult = propositionItem.GetTopItemResult();
                topItemResult.ExpandResultItem();

                topItemResult.IsAnyPropositionFeatureContainsCurrencySymbol(currencySymbol)
                    .Should().BeTrue("Символ валюты должен отображаться в каком-либо поле инфы по карте");
            }
        }
    }
}