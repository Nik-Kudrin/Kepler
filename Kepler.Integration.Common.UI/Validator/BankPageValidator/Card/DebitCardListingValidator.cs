using Kepler.Integration.Common.UI.Page.Bank.DebitCard;

namespace Kepler.Integration.Common.UI.Validator.BankPageValidator.Card
{
    public class DebitCardListingValidator : CardListingValidator<DebitCardListingPage, DebitCardPropositionItem>
    {
        public DebitCardListingValidator()
        {
        }

        public DebitCardListingValidator(DebitCardListingPage page)
            : base(page)
        {
        }
    }
}