using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Kepler.Common.Error;
using Kepler.Common.Models;
using Kepler.Core;
using Kepler.Core.Common;
using Kepler.Models;
using Kepler.Service.Config;
using Newtonsoft.Json;

namespace Kepler.Service
{
    public class ConfigImporter
    {
        private ConfigMapper mapper = new ConfigMapper();

        /// <summary>
        /// Import json config file
        /// </summary>
        /// <param name="jsonConfig"></param>
        /// <returns>Return empty string, if import was 'Ok', otherwise - error message</returns>
        public string ImportConfig(string jsonConfig)
        {
            TestImportConfig importedConfig;
            try
            {
                importedConfig = JsonConvert.DeserializeObject<TestImportConfig>(jsonConfig);
            }
            catch (Exception ex)
            {
                return
                    new ErrorMessage() {Code = ErrorMessage.ErorCode.ParsingFileError, ExceptionMessage = ex.Message}
                        .ToString();
            }

            var validationErrorMessage = ValidateImportedConfigObjects(importedConfig);
            if (validationErrorMessage != "")
                return validationErrorMessage;

            var mappedProjects = mapper.GetProjects(importedConfig.Projects).ToList();

            try
            {
                mappedProjects = BindImportedProjectWithExistedInDB(mappedProjects);
            }
            catch (Exception ex)
            {
                return ex.Message;
            }

            List<Branch> branches = null;
            try
            {
                branches = BindBranchesWithProjects(importedConfig, mappedProjects);
            }
            catch (Exception ex)
            {
                return ex.Message;
            }

            var builds = BindImportedBranchesWithBuilds(branches);
            var assemblies = BindTestAssembliesWithBuilds(importedConfig, branches);
            assemblies = BindTestSuitesWithAssemblies(importedConfig, assemblies);
            BindTestCasesWithSuites(importedConfig, assemblies);
            BindScreenshotsWithTestCases(importedConfig, branches, assemblies);

            return "";
        }


        private string ValidateImportedConfigObjects(TestImportConfig importedConfig)
        {
            var mapper = new ConfigMapper();
            var mappedProjects = mapper.GetProjects(importedConfig.Projects).ToList();

            // validate projects
            var validationMessage = ConfigValidator.ValidateProjects(mappedProjects);
            if (validationMessage != "")
                return validationMessage;

            // validate test assemblies
            foreach (var project in importedConfig.Projects)
            {
                foreach (var branchConfig in project.Branches)
                {
                    var assemblies = mapper.GetAssemblies(branchConfig.TestAssemblies);
                    validationMessage = ConfigValidator.ValidateTestAssemblies(assemblies.ToList());

                    if (validationMessage != "")
                        return validationMessage;
                }
            }


            foreach (var project in importedConfig.Projects)
            {
                foreach (var branchConfig in project.Branches)
                {
                    //validate suites
                    foreach (var testAssemblyConfig in branchConfig.TestAssemblies)
                    {
                        var items = mapper.GetSuites(testAssemblyConfig.TestSuites);
                        validationMessage = ConfigValidator.ValidateTestSuites(items.ToList());

                        if (validationMessage != "")
                            return validationMessage;


                        //validate cases
                        foreach (var testSuite in testAssemblyConfig.TestSuites)
                        {
                            var cases = mapper.GetCases(testSuite.TestCases);
                            validationMessage = ConfigValidator.ValidateTestCases(cases.ToList());

                            if (validationMessage != "")
                                return validationMessage;


                            //validate screenshots
                            foreach (var testCase in testSuite.TestCases)
                            {
                                validationMessage = ConfigValidator.ValidateScreenshots(testCase.ScreenShots);

                                if (validationMessage != "")
                                    return validationMessage;
                            }
                        }
                    }
                }
            }

            return "";
        }


        private List<Project> BindImportedProjectWithExistedInDB(List<Project> mappedProjects)
        {
            for (int index = 0; index < mappedProjects.Count(); index++)
            {
                var project = mappedProjects[index];

                var projectName = project.Name.Trim();
                var projectsFromDb = ProjectRepository.Instance.Find(projectName);

                if (projectsFromDb.Count() == 0 || projectsFromDb.Count() > 1)
                    throw new Exception(new ErrorMessage()
                    {
                        Code = ErrorMessage.ErorCode.ObjectNotFoundInDb,
                        ExceptionMessage =
                            $"Project '{projectName}' wasn't found in database (or there are more than 1 of them)"
                    }.ToString());

                mappedProjects[index] = projectsFromDb.FirstOrDefault();
            }

            return mappedProjects;
        }

        private List<Branch> BindBranchesWithProjects(TestImportConfig importedConfig, List<Project> mappedProjects)
        {
            var branches = new List<Branch>();

            foreach (var mappedProject in mappedProjects)
            {
                if (!mappedProject.MainBranchId.HasValue)
                    throw new Exception(new ErrorMessage()
                    {
                        Code = ErrorMessage.ErorCode.ProjectDontHaveMainBranch,
                        ExceptionMessage =
                            $"Project '{mappedProject.Name}' don't have main branch. Please, manually specify for project which branch should be considered as main."
                    }.ToString());


                var storedBranches = BranchRepository.Instance.Find(item => item.ProjectId == mappedProject.Id);
                var importedBranches = mapper.GetBranches(importedConfig.Projects
                    .First(project => project.Name == mappedProject.Name).Branches).ToList();

                var mainBranch = BranchRepository.Instance.Get(mappedProject.MainBranchId.Value);
                var mainBranchBaselineScreenShots =
                    ScreenShotRepository.Instance.GetBaselineScreenShots(mainBranch.BaseLineId.Value);

                for (int index = 0; index < importedBranches.Count; index++)
                {
                    var importedBranch = importedBranches[index];
                    var storedBranchEqualWithImporedBranch =
                        storedBranches.FirstOrDefault(branch => branch.Name == importedBranch.Name);

                    if (storedBranchEqualWithImporedBranch != null)
                    {
                        importedBranch = storedBranchEqualWithImporedBranch;
                    }
                    else // if branch don't exist in DB, copy a baseline from main branch and screenshots from baseline
                    {
                        importedBranch.ProjectId = mappedProject.Id;
                        BranchRepository.Instance.Insert(importedBranch);

                        var baseline = new BaseLine() {BranchId = importedBranch.Id};
                        BaseLineRepository.Instance.Insert(baseline);

                        importedBranch.BaseLineId = baseline.Id;
                        BranchRepository.Instance.UpdateAndFlashChanges(importedBranch);

                        CopyScreenShotsFromMainBranchBaselineToNewBaseline(baseline, mainBranchBaselineScreenShots);
                    }

                    branches.Add(importedBranch);
                }
            }

            return branches;
        }


        public static void CopyScreenShotsFromMainBranchBaselineToNewBaseline(BaseLine newBaseLine,
            IEnumerable<ScreenShot> mainBranchBaselineScreenShots)
        {
            foreach (var mainBaselineScreenShot in mainBranchBaselineScreenShots)
            {
                var newBaselineScreenShot = new ScreenShot()
                {
                    BaseLineId = newBaseLine.Id,
                    ImagePath = mainBaselineScreenShot.ImagePath,
                    Name = mainBaselineScreenShot.Name,
                    Status = mainBaselineScreenShot.Status
                };

                ScreenShotRepository.Instance.Insert(newBaselineScreenShot);
            }
        }

        /// <summary>
        /// Create new build for each branch and save them in DB
        /// </summary>
        /// <param name="branches"></param>
        /// <returns>List new bounded builds</returns>
        private List<Build> BindImportedBranchesWithBuilds(List<Branch> branches)
        {
            var builds = new List<Build>();

            foreach (var branch in branches)
            {
                var build = new Build() {Status = ObjectStatus.InQueue, BranchId = branch.Id};

                BuildRepository.Instance.Insert(build);
                builds.Add(build);

                branch.Builds.Add(build.Id, build);
                branch.LatestBuildId = build.Id;

                BranchRepository.Instance.UpdateAndFlashChanges(branch);
            }

            return builds;
        }

        /// <summary>
        /// Create new test assemblies. Bind each assembly with corresponding build and branch. Save assembly in DB
        /// </summary>
        /// <param name="importedConfig"></param>
        /// <param name="branches"></param>
        /// <returns>List of new bounded assemblies</returns>
        private List<TestAssembly> BindTestAssembliesWithBuilds(TestImportConfig importedConfig,
            IEnumerable<Branch> branches)
        {
            var assemblies = new List<TestAssembly>();

            foreach (var project in importedConfig.Projects)
            {
                foreach (var branchConfig in project.Branches)
                {
                    var mappedAssemblies = mapper.GetAssemblies(branchConfig.TestAssemblies).ToList();
                    var assemblyBranch = branches.FirstOrDefault(proj => proj.Name == branchConfig.Name);

                    for (int index = 0; index < mappedAssemblies.Count(); index++)
                    {
                        var mappedAssembly = mappedAssemblies[0];

                        mappedAssembly.ParentObjId = assemblyBranch.LatestBuildId;
                        mappedAssembly.BuildId = assemblyBranch.LatestBuildId;
                        mappedAssembly.Status = ObjectStatus.InQueue;

                        TestAssemblyRepository.Instance.Insert(mappedAssembly);
                    }

                    assemblies.AddRange(mappedAssemblies);
                }
            }

            return assemblies;
        }

        /// <summary>
        /// Create new test suites. Bind each suite with corresponding build and assembly. Save suite in DB
        /// </summary>
        /// <param name="importedConfig"></param>
        /// <param name="assemblies"></param>
        /// <returns>Return updated test assemblies (bounded with suites)</returns>
        private List<TestAssembly> BindTestSuitesWithAssemblies(TestImportConfig importedConfig,
            List<TestAssembly> assemblies)
        {
            foreach (var projectConfig in importedConfig.Projects)
            {
                foreach (var branchConfig in projectConfig.Branches)
                {
                    foreach (var testConfigAssembly in branchConfig.TestAssemblies)
                    {
                        var mappedSuites = mapper.GetSuites(testConfigAssembly.TestSuites).ToList();
                        var currentAssembly = assemblies.Find(item => item.Name == testConfigAssembly.Name);

                        for (int index = 0; index < mappedSuites.Count(); index++)
                        {
                            var suite = mappedSuites[index];

                            suite.BuildId = currentAssembly.BuildId;
                            suite.ParentObjId = currentAssembly.Id;
                            suite.Status = ObjectStatus.InQueue;

                            TestSuiteRepository.Instance.Insert(suite);
                        }

                        currentAssembly.TestSuites = mappedSuites.ToDictionary(item => item.Id, item => item);
                        TestAssemblyRepository.Instance.FlushChanges();
                    }
                }
            }

            return assemblies;
        }

        /// <summary>
        /// Create new test cases. Bind each test case with corresponding build, suite. Save test case in DB
        /// </summary>
        /// <param name="importedConfig"></param>
        /// <param name="assemblies"></param>
        /// <returns></returns>
        private void BindTestCasesWithSuites(TestImportConfig importedConfig, List<TestAssembly> assemblies)
        {
            // map cases
            // bind cases with suites
            // save cases in db

            foreach (var projectConfig in importedConfig.Projects)
            {
                foreach (var branchConfig in projectConfig.Branches)
                {
                    foreach (var testConfigAssembly in branchConfig.TestAssemblies)
                    {
                        var currentAssembly = assemblies.Find(item => item.Name == testConfigAssembly.Name);
                        foreach (var testSuiteConfig in testConfigAssembly.TestSuites)
                        {
                            var currentSuite =
                                currentAssembly.TestSuites.ToList()
                                    .Find(item => item.Value.Name == testSuiteConfig.Name);
                            var mappedCases = mapper.GetCases(testSuiteConfig.TestCases).ToList();

                            for (int index = 0; index < mappedCases.Count; index++)
                            {
                                var testCase = mappedCases[index];
                                testCase.Status = ObjectStatus.InQueue;
                                testCase.BuildId = currentAssembly.BuildId;
                                testCase.ParentObjId = currentSuite.Key;

                                TestCaseRepository.Instance.Insert(testCase);
                            }

                            currentSuite.Value.TestCases = mappedCases.ToDictionary(item => item.Id, item => item);
                            TestSuiteRepository.Instance.FlushChanges();
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Create new screenshots. Bind each screenshot with corresponding build, suite. Save test case in DB
        /// </summary>
        /// <param name="importedConfig"></param>
        /// <param name="branches"></param>
        /// <param name="assemblies"></param>
        private void BindScreenshotsWithTestCases(TestImportConfig importedConfig, List<Branch> branches,
            List<TestAssembly> assemblies)
        {
            foreach (var projectConfig in importedConfig.Projects)
            {
                foreach (var branchConfig in projectConfig.Branches)
                {
                    var currentBranch = branches.Find(item => item.Name == branchConfig.Name);

                    foreach (var testConfigAssembly in branchConfig.TestAssemblies)
                    {
                        var currentAssembly = assemblies.Find(item => item.Name == testConfigAssembly.Name);
                        foreach (var testSuiteConfig in testConfigAssembly.TestSuites)
                        {
                            var currentSuite =
                                currentAssembly.TestSuites.ToList()
                                    .Find(item => item.Value.Name == testSuiteConfig.Name);
                            foreach (var testCaseConfig in testSuiteConfig.TestCases)
                            {
                                var currentCase =
                                    currentSuite.Value.TestCases.ToList()
                                        .Find(item => item.Value.Name == testCaseConfig.Name);
                                var screenShots = testCaseConfig.ScreenShots;

                                for (int index = 0; index < screenShots.Count; index++)
                                {
                                    var screenShot = screenShots[index];
                                    screenShot.BuildId = currentCase.Value.BuildId;
                                    screenShot.ParentObjId = currentCase.Key;
                                    screenShot.BaseLineId = currentBranch.BaseLineId.Value;
                                    screenShot.Status = ObjectStatus.InQueue;

                                    ScreenShotRepository.Instance.Insert(screenShot);
                                }

                                currentCase.Value.ScreenShots = screenShots.ToDictionary(item => item.Id, item => item);
                                TestCaseRepository.Instance.FlushChanges();
                            }
                        }
                    }
                }
            }
        }
    }
}