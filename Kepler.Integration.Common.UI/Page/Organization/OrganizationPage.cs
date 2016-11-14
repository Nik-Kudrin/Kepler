using Kepler.Integration.Common.UI.Page.Common;
using Kepler.Integration.Common.UI.Validator;
using OpenQA.Selenium;

namespace Kepler.Integration.Common.UI.Page.Organization
{
    public class OrganizationPage : CommonPage
    {
        public string GetOrganizationNameFromH1()
        {
            return Find.Element(By.CssSelector("div.company-header-heading-container > h1")).Text;
        }

        public OrganizationMenuComponent OrganizationMenu()
        {
            return GetComponent<OrganizationMenuComponent>();
        }

        public OrganizationRatingComponent OrganizationRating()
        {
            return GetComponent<OrganizationRatingComponent>();
        }

        public virtual OrganizationPageValidator GetPageValidator()
        {
            return new OrganizationPageValidator(this);
        }
    }
}