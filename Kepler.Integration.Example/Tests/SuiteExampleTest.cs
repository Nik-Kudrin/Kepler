using System.Collections.Generic;
using Kepler.Integration.Common.JsonHelpers;
using Kepler.Integration.Common.Screens;
using Kepler.Integration.Common.UI;
using Kepler.Integration.Common.UI.Page.Common;

namespace Kepler.Integration.Example.Tests
{
    [TestSuiteScreen]
    public class SuiteExampleTest : InitSuite
    {
        public SuiteExampleTest(HostForTask hostForTask) : base(hostForTask)
        {
        }

        [TestCaseScreen]
        [DebugTest]
        public void SimpleWithDebugAttributeTest(TestCase testCase)
        {
            var listUrl = new List<string>
            {
                "/relative_to_site_url1/",
                "/relative_to_site_url2/"
            };

            GoToPageAndTakeScreen(testCase, listUrl);
        }


        [TestCaseScreen]
        public void SimpleTest(TestCase testCase)
        {
            var listUrl = new List<string>
            {
                "/url_1/",
                "/url2/2/",
            };

            GoToPageAndTakeScreen(testCase, listUrl);
        }


        [TestCaseScreen]
        public void MoreComplexTest(TestCase testCase)
        {
            var listUrl = new List<string>
            {
                "/url1/1/",
                "/url2/"
            };

            foreach (var url in listUrl)
            {
                HostForTask.CleanBeforeTest();
                HostForTask.GoToPage<CommonPage>(Config.ServerUnderTest + url);
                HostForTask.TakeScreenShot(testCase, url);
            }
        }
    }
}