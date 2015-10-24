using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Kepler.Common.CommunicationContracts;
using Kepler.Common.Error;
using Kepler.Common.Models;
using Kepler.Common.Models.Common;
using Kepler.Common.Repository;
using Kepler.Service.Core;
using Kepler.Service.RestWorkerClient;

namespace Kepler.Service
{
    public class KeplerService : IKeplerService
    {
        private BuildRepository buildRepo = BuildRepository.Instance;
        private TestCaseRepository testCaseRepo = TestCaseRepository.Instance;
        private TestAssemblyRepository assemblyRepository = TestAssemblyRepository.Instance;
        private ProjectRepository projectRepository = ProjectRepository.Instance;
        private TestSuiteRepository testSuiteRepo = TestSuiteRepository.Instance;
        private ImageWorkerRepository workerRepository = ImageWorkerRepository.Instance;

        // do not remove this field (used for build executor init)
        private static BuildExecutor _executor = BuildExecutor.GetExecutor();

        #region Common Actions

        private long ConvertStringToLong(string number)
        {
            return Convert.ToInt64(number);
        }

        public string RunOperation(string typeName, long objId, string operationName)
        {
            string exceptionMessage;

            switch (operationName.ToLowerInvariant())
            {
                case "run":
                    exceptionMessage = SetStatus(typeName, objId, ObjectStatus.InQueue);
                    if (exceptionMessage != "")
                        return exceptionMessage;

                    break;
                case "stop":
                    exceptionMessage = SetStatus(typeName, objId, ObjectStatus.Stopped);
                    if (exceptionMessage != "")
                        return exceptionMessage;


                    // TODO: implement 'Stop' method on image processor
                    // TODO: add logic inside 'UpdateScreenshots' method (if current status = Stopped , then just update diff image path field)

                    break;
                default:
                    return
                        $"OperationName {operationName} is not recognized. Possible values: run, stop";
            }

            return "";
        }

        private string SetStatus(string typeName, long objId, ObjectStatus newStatus)
        {
            switch (typeName.ToLowerInvariant())
            {
                case "build":
                    ObjectStatusUpdater.RecursiveSetObjectsStatus<Build>(objId, newStatus);
                    break;
                case "testAssembly":
                    ObjectStatusUpdater.RecursiveSetObjectsStatus<TestAssembly>(objId, newStatus);
                    break;
                case "testSuite":
                    ObjectStatusUpdater.RecursiveSetObjectsStatus<TestSuite>(objId, newStatus);
                    break;
                case "testCase":
                    ObjectStatusUpdater.RecursiveSetObjectsStatus<TestCase>(objId, newStatus);
                    break;
                case "screenShot":
                    ObjectStatusUpdater.RecursiveSetObjectsStatus<ScreenShot>(objId, newStatus);
                    break;

                default:
                    return
                        $"TypeName {typeName} is not recognized. Possible values: build, testCase, testSuite, testAssembly, screenShot";
            }

            return "";
        }

        public string SetStatus(string typeName, long objId, string newStatus)
        {
            ObjectStatus status;
            switch (newStatus.ToLowerInvariant())
            {
                case "failed":
                    status = ObjectStatus.Failed;
                    break;
                case "passed":
                    status = ObjectStatus.Passed;
                    break;
                default:
                    return
                        $"Status {newStatus} is not recognized. Possible values: failed, passed";
            }

            return SetStatus(typeName, objId, status);
        }

        #endregion

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
                BaseLineRepository.Instance.UpdateAndFlashChanges(baseline);

                long mainBaseLineId;

                if (!project.MainBranchId.HasValue)
                {
                    mainBaseLineId = baseline.Id;
                }
                else
                {
                    mainBaseLineId = BranchRepository.Instance.Get(project.MainBranchId.Value).BaseLineId.Value;
                }

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


            if (!project.MainBranchId.HasValue &&
                BranchRepository.Instance.Find(branch => branch.ProjectId == project.Id).Any())
            {
                return new ErrorMessage()
                {
                    Code = ErrorMessage.ErorCode.ProjectDontHaveMainBranch,
                    ExceptionMessage =
                        $"Project '{project.Name}' don't have main branch. Please, manually specify for project which branch should be considered as main."
                }.ToString();
            }

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
            if (name != newName)
            {
                var branchExistMessage = IsBranchAlreadyExist(newName);
                if (branchExistMessage != "")
                    return branchExistMessage;
            }

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

                var project = ProjectRepository.Instance.Get(branch.ProjectId.Value);
                project.MainBranchId = branch.Id;
                ProjectRepository.Instance.UpdateAndFlashChanges(project);
            }

            branch.Name = newName;
            branch.IsMainBranch = isMainBranch;
            BranchRepository.Instance.UpdateAndFlashChanges(branch);

            return string.Empty;
        }

        #endregion

        public string ImportTestConfig(string testConfig)
        {
            var configImporter = new ConfigImporter();
            return configImporter.ImportConfig(testConfig);
        }

        public void UpdateScreenShots(ImageComparisonContract imageComparisonContract)
        {
            foreach (var imageComparisonInfo in imageComparisonContract.ImageComparisonList)
            {
                var screenShot = ScreenShotRepository.Instance.Get(imageComparisonInfo.ScreenShotId);

                if (imageComparisonInfo.IsImagesDifferent || imageComparisonInfo.ErrorMessage != "")
                {
                    screenShot.Status = ObjectStatus.Failed;
                    screenShot.ErrorMessage = imageComparisonInfo.ErrorMessage;
                }
                else
                {
                    screenShot.Status = ObjectStatus.Passed;
                    screenShot.IsLastPassed = true;

                    if (imageComparisonInfo.LastPassedScreenShotId.HasValue)
                    {
                        var oldPassedScreenShot =
                            ScreenShotRepository.Instance.Get(imageComparisonInfo.LastPassedScreenShotId.Value);
                        oldPassedScreenShot.IsLastPassed = false;
                        ScreenShotRepository.Instance.Update(oldPassedScreenShot);
                    }
                }

                screenShot.DiffImagePath = imageComparisonInfo.DiffImgPathToSave;
                ScreenShotRepository.Instance.Update(screenShot);
            }

            ScreenShotRepository.Instance.FlushChanges();
        }

        #region ImageWorkers

        public IEnumerable<ImageWorker> GetImageWorkers()
        {
            return workerRepository.FindAll();
        }


        public string RegisterImageWorker(string name, string imageWorkerServiceUrl)
        {
            if (!workerRepository.Find(imageWorkerServiceUrl).Any())
            {
                workerRepository.Insert(new ImageWorker()
                {
                    Name = name,
                    WorkerServiceUrl = imageWorkerServiceUrl
                });

                var restImageWorkerClient = new RestImageProcessorClient(imageWorkerServiceUrl);
                restImageWorkerClient.SetDiffImagePath();
            }
            else
            {
                return new ErrorMessage()
                {
                    Code = ErrorMessage.ErorCode.NotUniqueObjects,
                    ExceptionMessage = $"Image worker with the same URL {imageWorkerServiceUrl} already exist"
                }.ToString();
            }

            return string.Empty;
        }

        public string UpdateImageWorker(string name, string newName, string newWorkerServiceUrl)
        {
            var worker = workerRepository.Find(item => item.Name == name).FirstOrDefault();

            if (worker == null)
            {
                return new ErrorMessage()
                {
                    Code = ErrorMessage.ErorCode.ObjectNotFoundInDb,
                    ExceptionMessage = $"Image worker with name {name} not found"
                }.ToString();
            }
            else
            {
                worker.Name = newName;
                worker.WorkerServiceUrl = newWorkerServiceUrl;

                workerRepository.UpdateAndFlashChanges(worker);
            }

            return string.Empty;
        }

        #endregion

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

            BuildExecutor.DiffImageSavingPath = diffImageSavingPath;
            BuildExecutor.GetExecutor().UpdateKeplerServiceUrlOnWorkers();
        }

        #endregion
    }
}