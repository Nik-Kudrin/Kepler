using Kepler.Integration.Common.UI.Page.Bank.CreditCard;
using Kepler.Integration.Common.UI.Validator.BankPageValidator.Card;

namespace Kepler.Integration.Common.UI.Page.Bank.DebitCard
{
    public class DebitCardListingPage : CardListingPage
    {
        public override T GetPageValidator<T, X>()
        {
            return new DebitCardListingValidator(this) as T;
        }
    }
}