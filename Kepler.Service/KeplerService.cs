using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Kepler.Common.Core;
using Kepler.Common.Error;
using Kepler.Common.Models;
using Kepler.Core;
using Kepler.Models;
using Kepler.Service.Core;

namespace Kepler.Service
{
    public class KeplerService : IKeplerService
    {
        private BuildRepository buildRepo = BuildRepository.Instance;
        private TestCaseRepository testCaseRepo = TestCaseRepository.Instance;
        private TestAssemblyRepository assemblyRepository = TestAssemblyRepository.Instance;
        private ProjectRepository projectRepository = ProjectRepository.Instance;
        private TestSuiteRepository testSuiteRepo = TestSuiteRepository.Instance;
        private ScreenShotRepository screenShotRepository = ScreenShotRepository.Instance;
        private ImageWorkerRepository workerRepository = ImageWorkerRepository.Instance;
        private static BuildExecutor _executor = BuildExecutor.GetExecutor();


        private long ConvertStringToLong(string number)
        {
            return Convert.ToInt64(number);
        }

        public Build GetBuild(string id)
        {
            return buildRepo.Get(ConvertStringToLong(id));
        }

        public IEnumerable<Build> GetBuilds()
        {
            try
            {
                return buildRepo.FindAll();
            }
            catch (Exception ex)
            {
                EventLog.WriteEntry("Kepler.Service", $"Build error message: {ex.Message}. StackTrace: {ex.StackTrace}");
            }
            return null;
        }

        #region TestCase

        public TestCase GetTestCase(string id)
        {
            return testCaseRepo.Get(ConvertStringToLong(id));
        }

        public IEnumerable<TestCase> GetTestCases(string testSuiteId)
        {
            return testCaseRepo.Find(ConvertStringToLong(testSuiteId));
        }

        #endregion

        #region TestSuite

        public TestSuite GetTestSuite(string id)
        {
            return testSuiteRepo.Get(ConvertStringToLong(id));
        }

        public IEnumerable<TestSuite> GetTestSuites(string assemblyId)
        {
            return testSuiteRepo.Find(ConvertStringToLong(assemblyId));
        }

        #endregion

        #region TestAssembly

        public TestAssembly GetTestAssembly(string assemblyId)
        {
            return assemblyRepository.Get(ConvertStringToLong(assemblyId));
        }


        public IEnumerable<TestAssembly> GetTestAssemblies(string buildId)
        {
            return assemblyRepository.Find(ConvertStringToLong(buildId));
        }

        #endregion

        #region Project

        public Project GetProject(string id)
        {
            return projectRepository.Get(ConvertStringToLong(id));
        }

        public IEnumerable<Project> GetProjects()
        {
            return projectRepository.FindAll();
        }

        public string CreateProject(string name)
        {
            if (projectRepository.Find(name).Any())
                return new ErrorMessage()
                {
                    Code = ErrorMessage.ErorCode.NotUniqueObjects,
                    ExceptionMessage = $"Project with name {name} already exist"
                }.ToString();

            try
            {
                var project = new Project() {Name = name};
                projectRepository.Insert(project);
            }
            catch (Exception ex)
            {
                return new ErrorMessage()
                {
                    Code = ErrorMessage.ErorCode.UndefinedError,
                    ExceptionMessage = $"Something bad happend. Exception: {ex.Message} {ex.StackTrace}"
                }.ToString();
            }

            return string.Empty;
        }

        #endregion

        #region Branch

        public string CreateBranch(string name, long projectId)
        {
            var validationMessage = ValidateBranchBeforeCreation(name, projectId);
            if (validationMessage != "")
                return validationMessage;

            var project = ProjectRepository.Instance.Get(projectId);
            try
            {
                var baseline = new BaseLine();
                BaseLineRepository.Instance.Insert(baseline);

                var branch = new Branch() {Name = name, BaseLineId = baseline.Id, ProjectId = projectId};
                BranchRepository.Instance.Insert(branch);

                baseline.BranchId = branch.Id;
                BaseLineRepository.Instance.Update(baseline);
                BaseLineRepository.Instance.FlushChanges();


                var mainBaseLineId = BranchRepository.Instance.Get(project.MainBranchId.Value).BaseLineId.Value;
                var mainBranchBaselineScreenShots =
                    ScreenShotRepository.Instance.GetBaselineScreenShots(mainBaseLineId);

                ConfigImporter.CopyScreenShotsFromMainBranchBaselineToNewBaseline(baseline,
                    mainBranchBaselineScreenShots);
            }
            catch (Exception ex)
            {
                return new ErrorMessage()
                {
                    Code = ErrorMessage.ErorCode.UndefinedError,
                    ExceptionMessage = $"Something bad happend. Exception: {ex.Message} {ex.StackTrace}"
                }.ToString();
            }

            return string.Empty;
        }


        private string ValidateBranchBeforeCreation(string branchName, long projectId)
        {
            var branchExistMessage = IsBranchAlreadyExist(branchName);
            if (branchExistMessage != "")
                return branchExistMessage;

            var project = ProjectRepository.Instance.Get(projectId);
            if (project == null)
            {
                return new ErrorMessage()
                {
                    Code = ErrorMessage.ErorCode.ObjectNotFoundInDb,
                    ExceptionMessage = $"Project with specified projectID {projectId} doesn't exist"
                }.ToString();
            }

            if (!project.MainBranchId.HasValue)
                return new ErrorMessage()
                {
                    Code = ErrorMessage.ErorCode.ProjectDontHaveMainBranch,
                    ExceptionMessage =
                        $"Project '{project.Name}' don't have main branch. Please, manually specify for project which branch should be considered as main."
                }.ToString();

            return string.Empty;
        }

        private string IsBranchAlreadyExist(string branchName)
        {
            if (BranchRepository.Instance.Find(branchName).Any())
            {
                return new ErrorMessage()
                {
                    Code = ErrorMessage.ErorCode.NotUniqueObjects,
                    ExceptionMessage = $"Branch with name {branchName} already exist"
                }.ToString();
            }

            return string.Empty;
        }

        public string UpdateBranch(string name, string newName, bool isMainBranch)
        {
            var branchExistMessage = IsBranchAlreadyExist(newName);
            if (branchExistMessage != "")
                return branchExistMessage;

            var branch = BranchRepository.Instance.Find(name).FirstOrDefault();

            if (branch == null)
            {
                return new ErrorMessage()
                {
                    Code = ErrorMessage.ErorCode.ObjectNotFoundInDb,
                    ExceptionMessage = $"Branch with name {name} doesn't exist"
                }.ToString();
            }

            if (isMainBranch)
            {
                var allProjectBranches =
                    BranchRepository.Instance.Find(item => item.ProjectId == branch.ProjectId).ToList();

                allProjectBranches.ForEach(item => item.IsMainBranch = false);
                BranchRepository.Instance.Update(allProjectBranches);
            }

            branch.Name = newName;
            branch.IsMainBranch = isMainBranch;
            BranchRepository.Instance.Update(branch);

            BranchRepository.Instance.FlushChanges();

            return string.Empty;
        }

        #endregion

        public string ImportTestConfig(string testConfig)
        {
            var configImporter = new ConfigImporter();
            return configImporter.ImportConfig(testConfig);
        }

        public void RegisterImageWorker(string imageWorkerServiceUrl)
        {
            if (workerRepository.Find(imageWorkerServiceUrl).Count() == 0)
                workerRepository.Insert(new ImageWorker() {WorkerServiceUrl = imageWorkerServiceUrl});
        }

        #region Kepler Configs

        public string GetDiffImageSavingPath()
        {
            var diffImgPathToSaveProperty = KeplerSystemConfigRepository.Instance.Find("DiffImgPathToSave");
            return diffImgPathToSaveProperty == null ? "" : diffImgPathToSaveProperty.Value;
        }

        public void SetDiffImageSavingPath(string diffImageSavingPath)
        {
            var diffImgPathToSaveProperty = KeplerSystemConfigRepository.Instance.Find("DiffImgPathToSave");

            if (diffImgPathToSaveProperty == null)
                KeplerSystemConfigRepository.Instance.Insert(new KeplerSystemConfig("DiffImgPathToSave",
                    diffImageSavingPath));
            else
            {
                diffImgPathToSaveProperty.Value = diffImageSavingPath;
                KeplerSystemConfigRepository.Instance.Update(diffImgPathToSaveProperty);
                KeplerSystemConfigRepository.Instance.FlushChanges();
            }
        }

        #endregion
    }
}