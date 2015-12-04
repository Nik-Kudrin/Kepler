using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.ServiceModel;
using System.ServiceModel.Activation;
using AutoMapper.Internal;
using Kepler.Common.CommunicationContracts;
using Kepler.Common.Error;
using Kepler.Common.Models;
using Kepler.Common.Models.Common;
using Kepler.Common.Repository;
using Kepler.Common.Util;
using Kepler.Service.Config;
using Kepler.Service.Core;
using Kepler.Service.RestWorkerClient;

namespace Kepler.Service
{
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    [ServiceBehavior(IncludeExceptionDetailInFaults = true)]
    public class KeplerService : IKeplerService
    {
        // do not remove this field (used for build executor init)
        private static BuildExecutor _executor = BuildExecutor.GetExecutor();

        public void ImportTestConfig(string testConfig)
        {
            var configImporter = new ConfigImporter();
            configImporter.ImportConfig(testConfig);
        }

        public void UpdateScreenShots(ImageComparisonContract imageComparisonContract)
        {
            foreach (var imageComparisonInfo in imageComparisonContract.ImageComparisonList)
            {
                var screenShot = ScreenShotRepository.Instance.Get(imageComparisonInfo.ScreenShotId);

                // if current screenshot status = Stopped, then just update diff image path field
                if (screenShot.Status == ObjectStatus.Stopped)
                {
                    screenShot.DiffImagePath = imageComparisonInfo.DiffImagePath;
                    screenShot.DiffPreviewPath = imageComparisonInfo.DiffPreviewPath;
                    screenShot.PreviewImagePath = imageComparisonInfo.SecondPreviewPath;
                    screenShot.BaseLinePreviewPath = imageComparisonInfo.FirstPreviewPath;

                    // Generate Url paths
                    UrlPathGenerator.ReplaceFilePathWithUrl(screenShot);

                    ScreenShotRepository.Instance.UpdateAndFlashChanges(screenShot);
                    continue;
                }

                // if Failed
                if (imageComparisonInfo.IsImagesDifferent || imageComparisonInfo.ErrorMessage != "")
                {
                    screenShot.Status = ObjectStatus.Failed;
                    screenShot.ErrorMessage = imageComparisonInfo.ErrorMessage;
                }
                else // if Passedd
                {
                    if (imageComparisonInfo.LastPassedScreenShotId.HasValue)
                    {
                        var oldPassedScreenShot =
                            ScreenShotRepository.Instance.Get(imageComparisonInfo.LastPassedScreenShotId.Value);
                        oldPassedScreenShot.IsLastPassed = false;
                        ScreenShotRepository.Instance.UpdateAndFlashChanges(oldPassedScreenShot);
                    }

                    screenShot.Status = ObjectStatus.Passed;
                    screenShot.IsLastPassed = true;
                }

                screenShot.DiffImagePath = imageComparisonInfo.DiffImagePath;
                screenShot.DiffPreviewPath = imageComparisonInfo.DiffPreviewPath;

                screenShot.PreviewImagePath = imageComparisonInfo.SecondPreviewPath;
                screenShot.BaseLinePreviewPath = imageComparisonInfo.FirstPreviewPath;

                // Generate Url paths
                UrlPathGenerator.ReplaceFilePathWithUrl(screenShot);

                ScreenShotRepository.Instance.UpdateAndFlashChanges(screenShot);
            }
        }

        #region Common Actions

        public void RunOperation(string typeName, long objId, string operationName)
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
                        LogErrorMessage(ErrorMessage.ErorCode.RunOperationError, ex);
                    }
                    break;

                case "stop":
                    var affectedScreenShots = new List<ScreenShot>();
                    try
                    {
                        affectedScreenShots = ObjectStatusUpdater.SetObjectsStatus(typeName, objId, ObjectStatus.Stopped);
                    }
                    catch (Exception ex)
                    {
                        LogErrorMessage(ErrorMessage.ErorCode.StopOperationError, ex);
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
                    LogErrorMessage(ErrorMessage.ErorCode.RunOperationError,
                        $"OperationName {operationName} is not recognized. Possible values: run, stop");
                    return;
            }
        }


        public void SetObjectsStatus(string typeName, long objId, string newStatus)
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
                    LogErrorMessage(ErrorMessage.ErorCode.SetObjectStatusError,
                        $"Status {newStatus} is not recognized. Possible values: failed, passed");
                    return;
            }

            try
            {
                ObjectStatusUpdater.SetObjectsStatus(typeName, objId, status);
            }
            catch (Exception ex)
            {
                LogErrorMessage(ErrorMessage.ErorCode.SetObjectStatusError, ex);
            }
        }

        #endregion

        #region Scheduler

        public DataSchedulerContract GetCleanDataScheduler(string schedulerName)
        {
            // check scheduler name
            switch (schedulerName)
            {
                case "buildCleanScheduler":
                case "logCleanScheduler":
                    break;
                default:
                    LogErrorMessage(ErrorMessage.ErorCode.SetObjectStatusError,
                        $"Scheduler name {schedulerName} is not recognized. Possible values: buildCleanScheduler, logCleanScheduler");
                    return null;
            }

            var schedulerProperty = GetKeplerConfigProperty(schedulerName);

            // if scheduler is'n initialized (first time, when applications start)
            if (string.IsNullOrEmpty(schedulerProperty))
            {
                var scheduler = new DataSchedulerContract()
                {
                    SchedulerName = schedulerName,
                    SchedulePeriod = TimeSpan.FromDays(30),
                    HistoryItemsNumberToPreserve = 3
                };

                var serializer = new RestSharpDataContractJsonSerializer();
                var serializedProperty = serializer.Serialize(scheduler);
                SetKeplerConfigProperty(schedulerName, serializedProperty);

                return scheduler;
            }

            return new RestSharpDataContractJsonDeserializer().Deserialize<DataSchedulerContract>(schedulerProperty);
        }


        public void UpdateCleanDataScheduler(DataSchedulerContract dataSchedulerContract)
        {
            /*  diffImageSavingPath = diffImageSavingPath.ToLowerInvariant();
            var diffImgPathToSaveProperty = KeplerSystemConfigRepository.Instance.Find("DiffImagePath");

            var previewPath = Path.Combine(diffImageSavingPath, "Preview");

            if (diffImgPathToSaveProperty == null)
            {
                KeplerSystemConfigRepository.Instance.Insert(new KeplerSystemConfig("DiffImagePath",
                    diffImageSavingPath));
                KeplerSystemConfigRepository.Instance.Insert(new KeplerSystemConfig("PreviewPath",
                    previewPath));
            }
            else
            {
                diffImgPathToSaveProperty.Value = diffImageSavingPath;

                var previewPathProperty = KeplerSystemConfigRepository.Instance.Find("PreviewPath");
                previewPathProperty.Value = previewPath;

                KeplerSystemConfigRepository.Instance.UpdateAndFlushChanges(diffImgPathToSaveProperty);
                KeplerSystemConfigRepository.Instance.UpdateAndFlushChanges(previewPathProperty);
                KeplerSystemConfigRepository.Instance.FlushChanges();
            }

            BuildExecutor.GetExecutor().UpdateKeplerServiceUrlOnWorkers();
            BuildExecutor.GetExecutor().UpdateDiffImagePath();
            UrlPathGenerator.DiffImagePath = new KeplerService().GetDiffImageSavingPath();
            UrlPathGenerator.PreviewImagePath = new KeplerService().GetPreviewSavingPath();*/
        }

        #endregion

        #region Build

        public Build GetBuild(long id)
        {
            var build = BuildRepository.Instance.Get(id);
            if (build == null)
                LogErrorMessage(ErrorMessage.ErorCode.ObjectNotFoundInDb, $"Build with ID {id} was not found");

            return build;
        }

        public IEnumerable<Build> GetBuilds(long branchId)
        {
            return BuildRepository.Instance.Find(item => item.BranchId == branchId);
        }

        public void DeleteBuild(long id)
        {
            RunOperation("build", id, "stop");
            DataCleaner.DeleteObjectsTreeRecursively<Build>(id, true);
        }

        #endregion

        #region ScreenShot

        public ScreenShot GetScreenShot(long id)
        {
            return ScreenShotRepository.Instance.Get(id);
        }

        public IEnumerable<ScreenShot> GetScreenShots(long testCaseId)
        {
            return ScreenShotRepository.Instance.Find(item => item.ParentObjId == testCaseId);
        }

        #endregion

        #region TestCase

        public TestCase GetTestCase(long id)
        {
            return TestCaseRepository.Instance.GetCompleteObject(id);
        }

        public IEnumerable<TestCase> GetTestCases(long testSuiteId)
        {
            return TestCaseRepository.Instance.GetObjectsTreeByParentId(testSuiteId);
        }

        #endregion

        #region TestSuite

        public TestSuite GetTestSuite(long id)
        {
            return TestSuiteRepository.Instance.GetCompleteObject(id);
        }

        public IEnumerable<TestSuite> GetTestSuites(long assemblyId)
        {
            return TestSuiteRepository.Instance.GetObjectsTreeByParentId(assemblyId);
        }

        #endregion

        #region TestAssembly

        public TestAssembly GetTestAssembly(long id)
        {
            return TestAssemblyRepository.Instance.GetCompleteObject(id);
        }


        public IEnumerable<TestAssembly> GetTestAssemblies(long buildId)
        {
            return TestAssemblyRepository.Instance.GetObjectsTreeByParentId(buildId);
        }

        #endregion

        #region Project

        public Project GetProject(long id)
        {
            return ProjectRepository.Instance.GetCompleteObject(id);
        }

        public IEnumerable<Project> GetProjects()
        {
            var projectRepo = ProjectRepository.Instance;
            var projects = projectRepo.FindAll();
            projects.Each(project => projectRepo.GetCompleteObject(project.Id));

            return projects;
        }

        public void CreateProject(string name)
        {
            var projectRepo = ProjectRepository.Instance;
            if (projectRepo.Find(name).Any())
                LogErrorMessage(ErrorMessage.ErorCode.NotUniqueObjects, $"Project with name {name} already exist");

            try
            {
                var project = new Project {Name = name};
                projectRepo.Insert(project);
            }
            catch (Exception ex)
            {
                LogErrorMessage(ErrorMessage.ErorCode.UndefinedError, ex);
            }
        }

        public void UpdateProject(long id, string newName)
        {
            var project = ProjectRepository.Instance.Get(id);

            if (project == null)
            {
                LogErrorMessage(ErrorMessage.ErorCode.ObjectNotFoundInDb, $"Project with Id={id} doesn't exist");
            }

            if (project.Name != newName)
            {
                if (ProjectRepository.Instance.Find(newName).Any())
                {
                    LogErrorMessage(ErrorMessage.ErorCode.NotUniqueObjects, $"Project with name {newName} already exist");
                }
            }

            project.Name = newName;
            ProjectRepository.Instance.UpdateAndFlashChanges(project);
        }

        public void DeleteProject(long id)
        {
            var branches = GetBranches(id);
            var builds = new List<long>();
            branches.Each(item => builds.AddRange(item.Builds.Keys));

            // Stop all builds
            builds.Each(buildId => RunOperation("build", buildId, "stop"));
            DataCleaner.DeleteObjectsTreeRecursively<Project>(id, true);
        }

        #endregion

        #region Branch

        public void CreateBranch(string name, long projectId)
        {
            ValidateBranchBeforeCreation(name, projectId);

            var project = ProjectRepository.Instance.Get(projectId);
            try
            {
                var baseline = new BaseLine();
                BaseLineRepository.Instance.Insert(baseline);

                var branch = new Branch
                {
                    Name = name,
                    BaseLineId = baseline.Id,
                    ProjectId = projectId
                };
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
                LogErrorMessage(ErrorMessage.ErorCode.UndefinedError, ex);
            }
        }


        private void ValidateBranchBeforeCreation(string branchName, long projectId)
        {
            IsBranchAlreadyExist(branchName);

            var project = ProjectRepository.Instance.Get(projectId);
            if (project == null)
            {
                LogErrorMessage(ErrorMessage.ErorCode.ObjectNotFoundInDb,
                    $"Project with specified projectID {projectId} doesn't exist");
            }


            if (!project.MainBranchId.HasValue &&
                BranchRepository.Instance.Find(branch => branch.ProjectId == project.Id).Any())
            {
                LogErrorMessage(ErrorMessage.ErorCode.ProjectDontHaveMainBranch,
                    $"Project '{project.Name}' don't have main branch. Please, manually specify for project which branch should be considered as main.");
            }
        }

        private void IsBranchAlreadyExist(string branchName)
        {
            if (BranchRepository.Instance.Find(branchName).Any())
            {
                LogErrorMessage(ErrorMessage.ErorCode.NotUniqueObjects, $"Branch with name {branchName} already exist");
            }
        }

        public void UpdateBranch(long id, string newName, bool isMainBranch)
        {
            var branch = BranchRepository.Instance.Get(id);

            if (branch == null)
            {
                LogErrorMessage(ErrorMessage.ErorCode.ObjectNotFoundInDb, $"Branch with Id={id} doesn't exist");
            }

            if (branch.Name != newName)
            {
                IsBranchAlreadyExist(newName);
            }

            if (isMainBranch)
            {
                var allProjectBranches =
                    BranchRepository.Instance.Find(item => item.ProjectId == branch.ProjectId).ToList();

                allProjectBranches.ForEach(item => item.IsMainBranch = false);
                BranchRepository.Instance.UpdateAndFlashChanges(allProjectBranches);

                var project = ProjectRepository.Instance.Get(branch.ProjectId.Value);
                project.MainBranchId = branch.Id;
                ProjectRepository.Instance.UpdateAndFlashChanges(project);
            }

            branch.Name = newName;
            branch.IsMainBranch = isMainBranch;
            BranchRepository.Instance.UpdateAndFlashChanges(branch);
        }

        public Branch GetBranch(long id)
        {
            var branch = BranchRepository.Instance.GetCompleteObject(id);
            if (branch == null)
                LogErrorMessage(ErrorMessage.ErorCode.ObjectNotFoundInDb, $"Branch with ID {id} was not found");

            return branch;
        }

        public IEnumerable<Branch> GetBranches(long projectId)
        {
            var branches = BranchRepository.Instance.Find(branch => branch.ProjectId == projectId);
            branches.Each(branch => branch.InitChildObjectsFromDb());

            return branches;
        }

        public void DeleteBranch(long id)
        {
            var builds = GetBuilds(id);
            // Stop all builds
            builds.Each(item => RunOperation("build", item.Id, "stop"));
            DataCleaner.DeleteObjectsTreeRecursively<Branch>(id, true);
        }

        #endregion

        #region ImageWorkers

        public IEnumerable<ImageWorker> GetImageWorkers()
        {
            return ImageWorkerRepository.Instance.FindAll();
        }


        public void RegisterImageWorker(string name, string imageWorkerServiceUrl)
        {
            var workerRepo = ImageWorkerRepository.Instance;
            if (!workerRepo.Find(imageWorkerServiceUrl).Any())
            {
                workerRepo.Insert(new ImageWorker
                {
                    Name = name,
                    WorkerServiceUrl = imageWorkerServiceUrl
                });

                var restImageWorkerClient = new RestImageProcessorClient(imageWorkerServiceUrl);
                restImageWorkerClient.SetKeplerServiceUrl();
            }
            else
            {
                LogErrorMessage(ErrorMessage.ErorCode.NotUniqueObjects,
                    "Image worker with the same URL {imageWorkerServiceUrl} already exist");
            }
        }

        public void UpdateImageWorker(string name, string newName, string newWorkerServiceUrl)
        {
            var workerRepo = ImageWorkerRepository.Instance;
            var worker = workerRepo.Find(item => item.Name == name).FirstOrDefault();

            if (worker == null)
            {
                LogErrorMessage(ErrorMessage.ErorCode.ObjectNotFoundInDb, $"Image worker with name {name} not found");
            }
            else
            {
                worker.Name = newName;
                worker.WorkerServiceUrl = newWorkerServiceUrl;
                workerRepo.UpdateAndFlashChanges(worker);
            }
        }

        public void DeleteImageWorker(long id)
        {
            var workerRepo = ImageWorkerRepository.Instance;
            var worker = workerRepo.Get(id);

            if (worker == null)
            {
                LogErrorMessage(ErrorMessage.ErorCode.ObjectNotFoundInDb, $"Image worker with ID {id} not found");
            }

            try
            {
                workerRepo.Delete(worker);
            }
            catch (Exception ex)
            {
                LogErrorMessage(ErrorMessage.ErorCode.UndefinedError, ex);
            }
        }

        #endregion

        #region Kepler Configs

        private string GetKeplerConfigProperty(string propertyName)
        {
            if (string.IsNullOrEmpty(propertyName))
                LogErrorMessage(ErrorMessage.ErorCode.UndefinedError, "Kepler config property name must be non emtpy");

            var property = KeplerSystemConfigRepository.Instance.Find(propertyName);
            return property == null ? "" : property.Value;
        }

        private void SetKeplerConfigProperty(string propertyName, string propertyValue)
        {
            var property = KeplerSystemConfigRepository.Instance.Find(propertyName);

            if (property == null)
            {
                KeplerSystemConfigRepository.Instance.Insert(new KeplerSystemConfig(propertyName, propertyValue));
            }
            else
            {
                property.Value = propertyValue;
                KeplerSystemConfigRepository.Instance.UpdateAndFlushChanges(property);
            }
        }

        public string GetDiffImageSavingPath()
        {
            return GetKeplerConfigProperty("DiffImagePath");
        }

        public string GetPreviewSavingPath()
        {
            return GetKeplerConfigProperty("PreviewPath");
        }

        public void SetDiffImageSavingPath(string diffImageSavingPath)
        {
            diffImageSavingPath = diffImageSavingPath.ToLowerInvariant();
            var previewPath = Path.Combine(diffImageSavingPath, "Preview");

            SetKeplerConfigProperty("DiffImagePath", diffImageSavingPath);
            SetKeplerConfigProperty("PreviewPath", previewPath);

            BuildExecutor.GetExecutor().UpdateKeplerServiceUrlOnWorkers();
            BuildExecutor.GetExecutor().UpdateDiffImagePath();
            UrlPathGenerator.DiffImagePath = new KeplerService().GetDiffImageSavingPath();
            UrlPathGenerator.PreviewImagePath = new KeplerService().GetPreviewSavingPath();
        }

        public string GetSourceImagePath()
        {
            return GetKeplerConfigProperty("SourceImagePath");
        }

        public void SetSourceImageSavingPath(string sourceImageSavingPath)
        {
            sourceImageSavingPath = sourceImageSavingPath.ToLowerInvariant();
            SetKeplerConfigProperty("SourceImagePath", sourceImageSavingPath);

            UrlPathGenerator.SourceImagePath = new KeplerService().GetSourceImagePath();
        }

        public string GetKeplerServiceUrl()
        {
            return GetKeplerConfigProperty("KeplerServiceUrl");
        }

        public void SetKeplerServiceUrl(string url)
        {
            SetKeplerConfigProperty("KeplerServiceUrl", url);

            BuildExecutor.KeplerServiceUrl = url;
        }

        #endregion

        #region Errors Logging

        public IEnumerable<ErrorMessage> GetErrors(DateTime fromTime)
        {
            return ErrorMessageRepository.Instance.Find(item => item.Time >= fromTime);
        }

        public IEnumerable<ErrorMessage> GetErrorsSinceLastViewed()
        {
            return ErrorMessageRepository.Instance.Find(item => item.IsLastViewed);
        }

        private void LogErrorMessage(ErrorMessage.ErorCode errorCode, string exceptionMessage)
        {
            var error = new ErrorMessage
            {
                Code = errorCode,
                ExceptionMessage = exceptionMessage
            };
            LogError(error);

            throw error.ConvertToWebFaultException(HttpStatusCode.InternalServerError);
        }

        private void LogErrorMessage(ErrorMessage.ErorCode errorCode, Exception exception)
        {
            LogErrorMessage(errorCode, $"{exception.Message}  {exception.StackTrace}");
        }

        public void LogError(ErrorMessage error)
        {
            ErrorMessageRepository.Instance.Insert(error);
        }

        public void SetLastViewedError(long errorId)
        {
            var error = ErrorMessageRepository.Instance.Get(errorId);
            if (error == null)
                throw new ErrorMessage
                {
                    Code = ErrorMessage.ErorCode.ObjectNotFoundInDb,
                    ExceptionMessage = $"Error message with id={errorId} not found"
                }.ConvertToWebFaultException(HttpStatusCode.InternalServerError);

            error.IsLastViewed = true;
            ErrorMessageRepository.Instance.UpdateAndFlashChanges(error);
        }

        #endregion
    }
}