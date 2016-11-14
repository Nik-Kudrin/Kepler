using Kepler.Integration.Common.UI.Page.Bank.CreditCard;

namespace Kepler.Integration.Common.UI.Validator.BankPageValidator.Card
{
    public class CreditCardListingValidator : CardListingValidator<CreditCardListingPage, CardPropositionItem>
    {
        public CreditCardListingValidator()
        {
        }

        public CreditCardListingValidator(CreditCardListingPage page)
            : base(page)
        {
        }
    }
}