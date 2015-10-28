using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Activation;
using Kepler.Common.CommunicationContracts;
using Kepler.Common.Error;
using Kepler.Common.Models;
using Kepler.Common.Models.Common;
using Kepler.Common.Repository;
using Kepler.Service.Config;
using Kepler.Service.Core;
using Kepler.Service.RestWorkerClient;

namespace Kepler.Service
{
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    [ServiceBehavior(IncludeExceptionDetailInFaults = true)]
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

        public string RunOperation(string typeName, long objId, string operationName)
        {
            switch (operationName.ToLowerInvariant())
            {
                case "run":
                    try
                    {
                        ObjectStatusUpdater.SetObjectsStatus(typeName, objId, ObjectStatus.InQueue);
                    }
                    catch (Exception ex)
                    {
                        return ex.Message;
                    }
                    break;
                case "stop":
                    List<ScreenShot> affectedScreenShots;

                    try
                    {
                        affectedScreenShots = ObjectStatusUpdater.SetObjectsStatus(typeName, objId, ObjectStatus.Stopped);
                    }
                    catch (Exception ex)
                    {
                        return ex.Message;
                    }

                    var workers = ImageWorkerRepository.Instance.FindAll()
                        .Where(worker => worker.WorkerStatus == ImageWorker.StatusOfWorker.Available).ToList();

                    foreach (var imageWorker in workers)
                    {
                        var restImageProcessorClient = new RestImageProcessorClient(imageWorker.WorkerServiceUrl);
                        restImageProcessorClient.StopStopDiffGeneration(affectedScreenShots);
                    }
                    break;
                default:
                    return
                        $"OperationName {operationName} is not recognized. Possible values: run, stop";
            }

            return "";
        }


        public string SetObjectsStatus(string typeName, long objId, string newStatus)
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

            try
            {
                ObjectStatusUpdater.SetObjectsStatus(typeName, objId, status);
            }
            catch (Exception ex)
            {
                return ex.Message;
            }

            return "";
        }

        #endregion

        public Build GetBuild(long id)
        {
            return buildRepo.Get(id);
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

        #region ScreenShot

        public ScreenShot GetScreenShot(long id)
        {
            return ScreenShotRepository.Instance.Get(id);
        }

        public IEnumerable<ScreenShot> GetScreenShots(long testCaseId)
        {
            return ScreenShotRepository.Instance.Find(item => item.ParentObjId == testCaseId);
        }

        public ImageComparisonInfo GetImageComparisonObj(long screenShotId)
        {
            throw new NotImplementedException();
        }

        public ImageComparisonContract GetImageComparisonObjects(long testCaseId)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region TestCase

        public TestCase GetTestCase(long id)
        {
            return testCaseRepo.Get(id);
        }

        public IEnumerable<TestCase> GetTestCases(long testSuiteId)
        {
            return testCaseRepo.Find(testSuiteId);
        }

        #endregion

        #region TestSuite

        public TestSuite GetTestSuite(long id)
        {
            return testSuiteRepo.Get(id);
        }

        public IEnumerable<TestSuite> GetTestSuites(long assemblyId)
        {
            return testSuiteRepo.Find(assemblyId);
        }

        #endregion

        #region TestAssembly

        public TestAssembly GetTestAssembly(long assemblyId)
        {
            return assemblyRepository.Get(assemblyId);
        }


        public IEnumerable<TestAssembly> GetTestAssemblies(long buildId)
        {
            return assemblyRepository.Find(buildId);
        }

        #endregion

        #region Project

        public Project GetProject(long id)
        {
            return projectRepository.Get(id);
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

                // if current screenshot status = Stopped, then just update diff image path field
                if (screenShot.Status == ObjectStatus.Stopped)
                {
                    screenShot.DiffImagePath = imageComparisonInfo.DiffImgPathToSave;
                    ScreenShotRepository.Instance.Update(screenShot);
                    continue;
                }

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