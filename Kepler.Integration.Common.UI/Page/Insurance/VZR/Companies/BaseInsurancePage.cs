using Kepler.Integration.Common.UI.Page.Common;

namespace Kepler.Integration.Common.UI.Page.Insurance.VZR.Companies
{
    public class BaseInsurancePage : CommonPage
    {
        public BaseInsurancePage()
        {
        }


        public virtual BaseInsurancePage WaitUntilPageIsLoaded()
        {
            return this;
        }

        public virtual void ValidatePageElement(string policyPrice)
        {
        }
    }
}