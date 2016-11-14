using Kepler.Integration.Common.UI.Page.Bank.Product;
using Kepler.Integration.Common.UI.Validator.BankPageValidator.Product;

namespace Kepler.Integration.Common.UI.Page.Bank.CreditCard
{
    public class CardListingPage : BankListingPage<CardPropositionItem>
    {
        public virtual T GetPropositions<T, Z, X>()
            where T : BasePropositionListingComponent<X>, IResult<Z, X>, new()
            where Z : CardPropositionComponent<X>, new()
            where X : CardPropositionItem, new()
        {
            return GetComponent<T>();
        }

        public new CreditCardSpecialPropositionListingComponent GetSpecialPropositions()
        {
            return GetComponent<CreditCardSpecialPropositionListingComponent>();
        }

        public new CreditCardStandardPropositionListingComponent GetPropositions()
        {
            return GetComponent<CreditCardStandardPropositionListingComponent>();
        }

        public new CardCalculatorComponent Calculator
        {
            get { return GetComponent<CardCalculatorComponent>(); }
        }

        public virtual T GetPageValidator<T, X>()
            where T : class, IValidateCard<X>, new()
            where X : CardPropositionItem, new()
        {
            return new T();
        }
    }
}