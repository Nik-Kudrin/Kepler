using FluentAssertions;
using Kepler.Integration.Common.UI.Page.Organization;
using Kepler.Integration.Common.UI.Validator.BankPageValidator;

namespace Kepler.Integration.Common.UI.Validator
{
    public class OrganizationPageValidator : BasePageValidator<OrganizationPage>, IValidateOrganizationPage
    {
        public OrganizationPageValidator(OrganizationPage page)
            : base(page)
        {
        }

        public void ValidateMenuExistOnPage(string organizationName)
        {
            Page.OrganizationMenu()
                .GetOrganizationNameInMenu()
                .ShouldBeEquivalentTo(organizationName, "Название организации не совпадает на странице организации");
        }


        public void ValidateAll(string organizationName)
        {
            ValidateOrganizationNameExistOnPage(organizationName);
        }

        public void ValidateOrganizationNameExistOnPage(string bankName)
        {
            Page.GetOrganizationNameFromH1()
                .Should()
                .BeEquivalentTo(bankName, "Страница организации должна содержать название организации");
        }
    }
}