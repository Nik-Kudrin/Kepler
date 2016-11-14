using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Castle.Core.Internal;
using Kepler.Integration.Common.Screens;
using Kepler.Integration.Common.UI;
using Newtonsoft.Json;

namespace Kepler.Integration.Common.JsonHelpers
{
    public class JsonWorker
    {
        private ScreenShotStructure _screenShotStructure;
        private TestAssembly _testAssembly;
        private readonly Assembly _assemblyWithTests;

        public ConfigForJson ConfigForJson { get; set; }

        public JsonWorker(Type typeFromAsssemblyWithTests, ConfigForJson configForJson)
        {
            _assemblyWithTests = Assembly.GetAssembly(typeFromAsssemblyWithTests);
            ConfigForJson = configForJson;
        }

        public List<TestCase> GenerateJsonStructure()
        {
            _screenShotStructure = new ScreenShotStructure {Projects = new List<Project>()};
            var project = new Project {Name = ConfigForJson.ProjectName, Branches = new List<Branch>()};
            var branch = new Branch {Name = ConfigForJson.BranchName, TestAssemblies = new List<TestAssembly>()};
            _testAssembly = new TestAssembly
            {
                Name = _assemblyWithTests.GetName().Name,
                TestSuites = new List<TestSuite>()
            };

            _screenShotStructure.Projects.Add(project);
            project.Branches.Add(branch);
            branch.TestAssemblies.Add(_testAssembly);

            return GenerateSuitesAndTestStructure();
        }

        private List<TestCase> GenerateSuitesAndTestStructure()
        {
            var testSuites = _assemblyWithTests.GetTypes().Where(x => x.HasAttribute<TestSuiteScreenAttribute>());

            var allTests = new List<TestCase>();

            foreach (var testSuiteType in testSuites)
            {
                var testSuite = new TestSuite {Name = testSuiteType.Name, TestCases = new List<TestCase>()};

                IEnumerable<MethodInfo> testCaseMethods = new List<MethodInfo>();
#if DEBUG
                  testCaseMethods =
                    testSuiteType.GetMethods().Where(x => x.HasAttribute<DebugTestAttribute>()).ToList();
             
#else
                testCaseMethods =
                    testSuiteType.GetMethods().Where(x => x.HasAttribute<TestCaseScreenAttribute>()).ToList();
#endif
                _testAssembly.TestSuites.Add(testSuite);

                foreach (var testCaseMethod in testCaseMethods)
                {
                    var testCase = new TestCase
                    {
                        UsedStatus = TaskUsageStatus.Free,
                        TestSuiteType = testSuiteType,
                        Method = testCaseMethod,
                        Name = testCaseMethod.Name,
                        ScreenShots = new List<ScreenShot>()
                    };
                    testSuite.TestCases.Add(testCase);
                    allTests.Add(testCase);
                }
            }

            return allTests;
        }

        public void GenerateFolders()
        {
            var branchPath = Path.Combine(Config.ScreenPath, ConfigForJson.BranchName);
            Directory.CreateDirectory(branchPath);

            var buildIdPath = Path.Combine(branchPath, "BuildId_" + ConfigForJson.BuildId);
            Directory.CreateDirectory(buildIdPath);

            var testAssemblyPath = Path.Combine(buildIdPath, _testAssembly.Name);
            Directory.CreateDirectory(testAssemblyPath);


            foreach (var testSuite in _testAssembly.TestSuites)
            {
                var testSuitePath = Path.Combine(testAssemblyPath, testSuite.Name);
                Directory.CreateDirectory(testSuitePath);

                foreach (var testCase in testSuite.TestCases)
                {
                    var testCasePath = Path.Combine(testSuitePath, testCase.Name);
                    Directory.CreateDirectory(testCasePath);
                    testCase.Path = testCasePath;
                }
            }
        }

        public void WriteJsonFile()
        {
            var fullPathToResultJson = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources",
                ConfigForJson.JsonResultFileName);

            var serJson = JsonConvert.SerializeObject(_screenShotStructure, Formatting.Indented);
            File.WriteAllText(fullPathToResultJson, serJson);
        }
    }
}