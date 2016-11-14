using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using System.Threading;
using Kepler.Integration.Common.JsonHelpers;
using Kepler.Integration.Common.UI;
using Kepler.Integration.Common.UI.Page.Common;
using NLog;
using OpenQA.Selenium.Support.Extensions;
using ILogger = NLog.ILogger;

namespace Kepler.Integration.Common.Screens
{
    public enum TaskUsageStatus
    {
        Free = 1,
        InProgress = 2,
        Finished = 3
    }

    public class HostForTask
    {
        private Host _hostSeleno;
        public ILogger Log { get; set; }
        public List<TestCase> TestsWithStatus;

        public Host SelenoHost
        {
            get { return _hostSeleno ?? new Host(); }
        }


        public HostForTask(List<TestCase> tests)
        {
            TestsWithStatus = tests;
        }

        public void StartSeleno()
        {
            _hostSeleno = SelenoHost;

            Log = LogManager.GetLogger(GetType().UnderlyingSystemType.Name);
            Log.Info("Start seleno host for new task");
        }

        public void DisposeSeleno()
        {
            Log.Info("Finish seleno host in task {0}", Thread.CurrentThread.ManagedThreadId);

            _hostSeleno._host.Dispose();
        }


        public void TakeScreenshotsInTask()
        {
            foreach (var test in TestsWithStatus)
            {
                if (test.UsedStatus == TaskUsageStatus.Free)
                {
                    test.UsedStatus = TaskUsageStatus.InProgress;
                    var testSuiteObj = Activator.CreateInstance(test.TestSuiteType, this);

                    test.Method.Invoke(testSuiteObj, new object[] {test});
                    test.Method = null; //что бы не было в json конфиге лишней информации
                    test.TestSuiteType = null; //что бы не было в json конфиге лишней информации

                    test.UsedStatus = TaskUsageStatus.Finished;
                }
            }
        }

        public void CleanBeforeTest()
        {
            _hostSeleno._host.Application.Browser.Manage().Cookies.DeleteAllCookies();
            _hostSeleno._host.Application.Browser.ExecuteJavaScript<object>("localStorage.clear()");
        }

        public T GoToPage<T>(string url) where T : CommonPage, new()
        {
            // if url is relative one
            if (!url.StartsWith("http"))
                url = Config.ServerUnderTest + url;

            try
            {
                var page = _hostSeleno._host.NavigateToInitialPage<T>(url);
                Log.Info("Go to url  {0} in task {1}", url, Thread.CurrentThread.ManagedThreadId);
                page.WaitUntilPageIsLoaded();

                return page;
            }
            catch (Exception ex)
            {
                Log.Error(string.Format("Error happend when go to url: {0} Message: {1}", url, ex.InnerException));
            }

            return null;
        }

        public void TakeScreenShot(TestCase testCase, string url, int screenNumber = 0)
        {
            var screen = new ScreenShot {Url = url};
            testCase.ScreenShots.Add(screen);

            var hash = screen.Url.GetHashCode().ToString();

            var screenNameByUrl = url;
            if (screenNameByUrl.Contains("?"))
                screenNameByUrl = screenNameByUrl.Substring(0, screen.Url.IndexOf("?", StringComparison.Ordinal));
            screenNameByUrl = screenNameByUrl.Replace(@"/", "_") + hash + "_Num_" + screenNumber + "_";

            var screenFileName = screenNameByUrl + Guid.NewGuid().ToString().Substring(0, 8) + ".png";
            var fullScreenPath = Path.Combine(testCase.Path, screenFileName);

            screen.Name = screenNameByUrl;
            screen.ImagePath = fullScreenPath;

            try
            {
                var screenshot = _hostSeleno._host.Application.Camera.ScreenshotTaker.GetScreenshot();
                screenshot.SaveAsFile(screen.ImagePath, ImageFormat.Png);
            }
            catch (Exception ex)
            {
                Log.Error(string.Format("Error happend taking and saving screenshot: {0} Message: {1}", url, ex.InnerException));
            }

            Log.Info("Take screen for url {0} {1}", url, screen.ImagePath);
        }
    }
}