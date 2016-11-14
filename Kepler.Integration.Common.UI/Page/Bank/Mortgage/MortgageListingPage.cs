using Kepler.Integration.Common.UI.Page.Bank.Credit;
using Kepler.Integration.Common.UI.Validator.BankPageValidator.Product;

namespace Kepler.Integration.Common.UI.Page.Bank.Mortgage
{
    public class MortgageListingPage : CreditListingPage
    {
        public new CreditListingPageValidator GetPageValidator()
        {
            return new CreditListingPageValidator(this);
        }
    }
}