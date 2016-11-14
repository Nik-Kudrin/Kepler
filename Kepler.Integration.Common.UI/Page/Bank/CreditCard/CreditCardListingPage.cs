using Kepler.Integration.Common.UI.Validator.BankPageValidator.Card;

namespace Kepler.Integration.Common.UI.Page.Bank.CreditCard
{
    public class CreditCardListingPage : CardListingPage
    {
        public override T GetPageValidator<T, X>()
        {
            return new CreditCardListingValidator(this) as T;
        }
    }
}