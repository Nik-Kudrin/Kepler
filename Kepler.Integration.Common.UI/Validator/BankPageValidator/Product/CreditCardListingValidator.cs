using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using Sravni.Common.UI.Page.Bank.CreditCard;

namespace Sravni.Common.UI.Validator.BankPageValidator.Product
{
    public class CreditCardListingValidator : BankPropositionListingPageValidator<CreditCardListingPage, CardPropositionItem>
    {
        public CreditCardListingValidator(CreditCardListingPage page)
            : base(page)
        {
        }

        protected new CreditCardStandardPropositionListingComponent GetStandartPropositionComponent()
        {
            return Page.GetStandardPropositionListingComponent<CreditCardStandardPropositionListingComponent>();
        }

        protected new CreditCardSpecialPropositionListingComponent GetSpecialPropositionComponent()
        {
            return Page.GetSpecialPropositionListingComponent<CreditCardSpecialPropositionListingComponent>();
        }


        public override void ValidateSorting(string sortingType, string stringSortOrder)
        {
            var sortingOrder = ConvertSortOrderStringToEnum(stringSortOrder);

            switch (sortingType)
            {
                case "0": // % ставке
                    ValidateSortingIsCorrect(item => item.GetTopItemResult().GetRate(), sortingOrder, true);
                    break;
                case "1": // льготному периоду
                    ValidateSortingIsCorrect(item => item.GetTopItemResult().GetPrivelegesPeriod(), sortingOrder);
                    break;
                case "2": // кредитному лимиту
                    ValidateSortingIsCorrect(item => item.GetTopItemResult().GetLimit(), sortingOrder, false);
                    break;
                case "3": // CashBack
                    ValidateSortingIsCorrect(item => item.GetTopItemResult().GetCashBack(), sortingOrder, true);
                    break;
                case "4": // количеству миль
                    break;
                case "5": // названию банка
                    break;
                case "6": // популярности
                    break;
                case "7": // оценке в отзывах
                    break;

                default:
                    throw new NotImplementedException(sortingType + " sorting type isn't implemented");
            }
        }

        protected void ValidateSortingIsCorrect(Func<CreditCardPropositionComponent, Tuple<float, float>> predicate,
            SortingOrder sortOrder, bool sortByFirstItem)
        {
            // example Tuple<22,1; 23,0> (range of credit card rates)
            var sourceList = GetStandartPropositionComponent().GetStandardPropositionResult()
                .Select(predicate).ToList();

            sourceList.ForEach(_ => _.Item2.Should()
                .BeGreaterOrEqualTo(_.Item1, "Второе значение для фильтруемого параметра в кредит. картах не должно быть больше первого"));

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

        protected void ValidateSortingIsCorrect<T>(Func<CreditCardPropositionComponent, T> predicate,
            SortingOrder sortOrder = SortingOrder.Descending)
        {
            var sourceList = GetStandartPropositionComponent().GetStandardPropositionResult()
                .Select(predicate).ToList();
            var sortedList = SortList(sourceList, sortOrder);

            ValidateListSequenceIsEqual(sourceList, sortedList);
        }
    }
}