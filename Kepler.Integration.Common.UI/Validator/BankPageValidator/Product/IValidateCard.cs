using Kepler.Integration.Common.UI.Page.Bank.CreditCard;
using Kepler.Integration.Common.UI.Page.Bank.Product;

namespace Kepler.Integration.Common.UI.Validator.BankPageValidator.Product
{
    public interface IValidateCard<X> where X : CardPropositionItem, new()
    {
        void ValidateSorting<M, Z>(string sortingType, string stringSortOrder)
            where M : StandardPropositionListingComponent<X>, IResult<Z, X>, new()
            where Z : CardPropositionComponent<X>, new();

        void ValidateCurrencySortingIsCorrectlyApplied(string currencySymbol);
        void ValidateCountPropositons(int expectedCount);
    }
}