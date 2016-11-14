using System;
using System.Collections.Generic;
using System.Configuration;
using System.Threading.Tasks;
using Kepler.Integration.Common;
using Kepler.Integration.Common.JsonHelpers;
using Kepler.Integration.Common.Screens;
using Kepler.Integration.Common.UI;
using Kepler.Integration.Example.Tests;

namespace Kepler.Integration.Example
{
    public class Runner
    {
        public static int TaskCount { get; set; }
        public static ConfigForJson ConfigForJson { get; set; }

        static Runner()
        {
            TaskCount = Int32.Parse(ConfigurationManager.AppSettings["taskCount"]);
            ConfigForJson = new ConfigForJson
            {
                ProjectName = ConfigurationManager.AppSettings["keplerProjectName"],
                BranchName = ConfigurationManager.AppSettings["branchName"],
                JsonResultFileName = ConfigurationManager.AppSettings["jsonResult"],
                BuildId = ConfigurationManager.AppSettings["buildId"]
            };
            Config.BrowserName = ConfigurationManager.AppSettings["browser"];
            Config.SeleniumHubUrl = ConfigurationManager.AppSettings["seleniumHubUrl"];
            Config.ScreenPath = ConfigurationManager.AppSettings["screenPath"];
            Config.ServerUnderTest = ConfigurationManager.AppSettings["site"];
            Config.SplitServerName();
        }


        public static void Main()
        {
            var tasksList = new List<Task>();
            var selenoInstances = new List<HostForTask>();


            var jsonWorker = new JsonWorker(typeof (SuiteExampleTest), ConfigForJson);

            var testsWithStatus = jsonWorker.GenerateJsonStructure();
            jsonWorker.GenerateFolders();

            for (var i = 0; i < TaskCount; i++)
            {
                var hostForTask = new HostForTask(testsWithStatus);
                hostForTask.StartSeleno();

                selenoInstances.Add(hostForTask);
                tasksList.Add(Task.Run((Action) hostForTask.TakeScreenshotsInTask));
            }

            Task.WaitAll(tasksList.ToArray());

            foreach (var selenoInstance in selenoInstances)
            {
                selenoInstance.DisposeSeleno();
            }

            jsonWorker.WriteJsonFile();
        }
    }
}