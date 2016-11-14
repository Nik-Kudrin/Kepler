using System;
using Kepler.Integration.Common.UI.Page.Common.Login;
using Kepler.Integration.Common.UI.Validator;
using NLog;

namespace Kepler.Integration.Common.UI.Page.Common
{
    public class CommonPage : BasePage
    {
        protected Logger LOG { get; set; }

        public CommonPage()
        {
            LOG = LogManager.GetLogger(this.GetType().UnderlyingSystemType.Name);
        }

        public LoginComponent LoginPopUp()
        {
            return GetComponent<LoginComponent>();
        }

        public AccountMenuComponent LoginMenu()
        {
            return GetComponent<AccountMenuComponent>();
        }

        public virtual BasePageValidator<CommonPage> GetPageValidator()
        {
            return new BasePageValidator<CommonPage>(this);
        }

        public void WaitUntilListingOverlayIsDisplayed(TimeSpan timeout = default(TimeSpan))
        {
            GetComponent<CommonComponent>().WaitUntilListingOverlayIsDisplayed(timeout);
        }

        public T GetPage<T>() where T : CommonPage, new()
        {
            return GetComponent<T>();
        }
    }
}