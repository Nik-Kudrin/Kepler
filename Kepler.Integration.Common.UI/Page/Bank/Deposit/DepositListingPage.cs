using Kepler.Integration.Common.UI.Page.Bank.Product;
using Kepler.Integration.Common.UI.Validator.BankPageValidator.Product;

namespace Kepler.Integration.Common.UI.Page.Bank.Deposit
{
    public class DepositListingPage : BankListingPage<PropositionItem>
    {
        public new BankPropositionListingPageValidator<DepositListingPage, PropositionItem> GetPageValidator()
        {
            return new BankPropositionListingPageValidator<DepositListingPage, PropositionItem>(this);
        }
    }
}