namespace Kepler.Integration.Common.UI.Validator.BankPageValidator
{
    public interface IValidateOrganizationPage
    {
        void ValidateAll(string organizationName);
        void ValidateMenuExistOnPage(string organizationName);
    }
}