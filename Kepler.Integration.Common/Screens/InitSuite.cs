using System.Collections.Generic;
using Kepler.Integration.Common.JsonHelpers;
using Kepler.Integration.Common.UI;
using Kepler.Integration.Common.UI.Page.Common;

namespace Kepler.Integration.Common.Screens
{
    public abstract class InitSuite
    {
        public HostForTask HostForTask;

        protected InitSuite(HostForTask hostForTask)
        {
            HostForTask = hostForTask;
        }

        protected void GoToPageAndTakeScreen(TestCase testCase, List<string> listUrl)
        {
            foreach (var url in listUrl)
            {
                HostForTask.CleanBeforeTest();
                HostForTask.GoToPage<CommonPage>(Config.ServerUnderTest + url);
                HostForTask.TakeScreenShot(testCase, url);
            }
        }
    }
}