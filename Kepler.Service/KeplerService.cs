using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;
using AutoMapper.Internal;
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
                        LogErrorMessage(ErrorMessage.ErorCode.RunOperationError, ex.Message);
                    }
                    break;

                case "stop":
                    List<ScreenShot> affectedScreenShots = new List<ScreenShot>();
                    try
                    {
                        affectedScreenShots = ObjectStatusUpdater.SetObjectsStatus(typeName, objId, ObjectStatus.Stopped);
                    }
                    catch (Exception ex)
                    {
                        LogErrorMessage(ErrorMessage.ErorCode.RunOperationError, ex.Message);
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
                    break;
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
                    throw new WebFaultException<string>(
                        $"Status {newStatus} is not recognized. Possible values: failed, passed",
                        HttpStatusCode.InternalServerError);
            }

            try
            {
                ObjectStatusUpdater.SetObjectsStatus(typeName, objId, status);
            }
            catch (Exception ex)
            {
                throw new WebFaultException<string>(ex.Message, HttpStatusCode.InternalServerError);
            }
        }

        #endregion

        public Build GetBuild(long id)
        {
            return buildRepo.Get(id);
        }

        public IEnumerable<Build> GetBuilds()
        {
            return buildRepo.FindAll();
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

        #endregion

        #region TestCase

        public TestCase GetTestCase(long id)
        {
            return testCaseRepo.GetCompleteObject(id);
        }

        public IEnumerable<TestCase> GetTestCases(long testSuiteId)
        {
            return testCaseRepo.GetObjectsTreeByParentId(testSuiteId);
        }

        #endregion

        #region TestSuite

        public TestSuite GetTestSuite(long id)
        {
            return testSuiteRepo.GetCompleteObject(id);
        }

        public IEnumerable<TestSuite> GetTestSuites(long assemblyId)
        {
            return testSuiteRepo.GetObjectsTreeByParentId(assemblyId);
        }

        #endregion

        #region TestAssembly

        public TestAssembly GetTestAssembly(long id)
        {
            return assemblyRepository.GetCompleteObject(id);
        }


        public IEnumerable<TestAssembly> GetTestAssemblies(long buildId)
        {
            return assemblyRepository.GetObjectsTreeByParentId(buildId);
        }

        #endregion

        #region Project

        public Project GetProject(long id)
        {
            return projectRepository.GetCompleteObject(id);
        }

        public IEnumerable<Project> GetProjects()
        {
            var projects = projectRepository.FindAll();
            projects.Each(project => projectRepository.GetCompleteObject(project.Id));

            return projects;
        }

        public void CreateProject(string name)
        {
            if (projectRepository.Find(name).Any())
                throw new ErrorMessage()
                {
                    Code = ErrorMessage.ErorCode.NotUniqueObjects,
                    ExceptionMessage = $"Project with name {name} already exist"
                }.ConvertToWebFaultException(HttpStatusCode.InternalServerError);

            try
            {
                var project = new Project() {Name = name};
                projectRepository.Insert(project);
            }
            catch (Exception ex)
            {
                throw new ErrorMessage()
                {
                    Code = ErrorMessage.ErorCode.UndefinedError,
                    ExceptionMessage = $"Something bad happend. Exception: {ex.Message} {ex.StackTrace}"
                }.ConvertToWebFaultException(HttpStatusCode.InternalServerError);
            }
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
                throw new ErrorMessage()
                {
                    Code = ErrorMessage.ErorCode.UndefinedError,
                    ExceptionMessage = $"Something bad happend. {ex.Message} {ex.StackTrace}"
                }.ConvertToWebFaultException(HttpStatusCode.InternalServerError);
            }
        }


        private void ValidateBranchBeforeCreation(string branchName, long projectId)
        {
            IsBranchAlreadyExist(branchName);

            var project = ProjectRepository.Instance.Get(projectId);
            if (project == null)
            {
                throw new ErrorMessage()
                {
                    Code = ErrorMessage.ErorCode.ObjectNotFoundInDb,
                    ExceptionMessage = $"Project with specified projectID {projectId} doesn't exist"
                }.ConvertToWebFaultException(HttpStatusCode.InternalServerError);
            }


            if (!project.MainBranchId.HasValue &&
                BranchRepository.Instance.Find(branch => branch.ProjectId == project.Id).Any())
            {
                throw new ErrorMessage()
                {
                    Code = ErrorMessage.ErorCode.ProjectDontHaveMainBranch,
                    ExceptionMessage =
                        $"Project '{project.Name}' don't have main branch. Please, manually specify for project which branch should be considered as main."
                }.ConvertToWebFaultException(HttpStatusCode.InternalServerError);
            }
        }

        private void IsBranchAlreadyExist(string branchName)
        {
            if (BranchRepository.Instance.Find(branchName).Any())
            {
                throw new ErrorMessage()
                {
                    Code = ErrorMessage.ErorCode.NotUniqueObjects,
                    ExceptionMessage = $"Branch with name {branchName} already exist"
                }.ConvertToWebFaultException(HttpStatusCode.InternalServerError);
            }
        }

        public void UpdateBranch(string name, string newName, bool isMainBranch)
        {
            if (name != newName)
            {
                IsBranchAlreadyExist(newName);
            }

            var branch = BranchRepository.Instance.Find(name).FirstOrDefault();

            if (branch == null)
            {
                throw new ErrorMessage()
                {
                    Code = ErrorMessage.ErorCode.ObjectNotFoundInDb,
                    ExceptionMessage = $"Branch with name {name} doesn't exist"
                }.ConvertToWebFaultException(HttpStatusCode.InternalServerError);
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
        }

        public Branch GetBranch(long id)
        {
            return BranchRepository.Instance.GetCompleteObject(id);
        }

        public IEnumerable<Branch> GetBranches(long projectId)
        {
            var branches = BranchRepository.Instance.Find(branch => branch.ProjectId == projectId);
            branches.Each(branch => branch.InitChildObjectsFromDb());

            return branches;
        }

        #endregion

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
                    screenShot.PreviewImagePath = imageComparisonInfo.FirstPreviewPath;
                    screenShot.BaseLinePreviewPath = imageComparisonInfo.SecondImagePath;
                    screenShot.DiffPreviewPath = imageComparisonInfo.DiffPreviewPath;
                    ScreenShotRepository.Instance.Update(screenShot);
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
                        ScreenShotRepository.Instance.Update(oldPassedScreenShot);
                    }

                    screenShot.Status = ObjectStatus.Passed;
                    screenShot.IsLastPassed = true;
                }

                screenShot.DiffImagePath = imageComparisonInfo.DiffImagePath;
                screenShot.PreviewImagePath = imageComparisonInfo.FirstPreviewPath;
                screenShot.BaseLinePreviewPath = imageComparisonInfo.SecondImagePath;
                screenShot.DiffPreviewPath = imageComparisonInfo.DiffPreviewPath;
                ScreenShotRepository.Instance.Update(screenShot);
            }

            ScreenShotRepository.Instance.FlushChanges();
        }

        #region ImageWorkers

        public IEnumerable<ImageWorker> GetImageWorkers()
        {
            return workerRepository.FindAll();
        }


        public void RegisterImageWorker(string name, string imageWorkerServiceUrl)
        {
            if (!workerRepository.Find(imageWorkerServiceUrl).Any())
            {
                workerRepository.Insert(new ImageWorker()
                {
                    Name = name,
                    WorkerServiceUrl = imageWorkerServiceUrl
                });

                var restImageWorkerClient = new RestImageProcessorClient(imageWorkerServiceUrl);
                restImageWorkerClient.SetKeplerServiceUrl();
            }
            else
            {
                throw new ErrorMessage()
                {
                    Code = ErrorMessage.ErorCode.NotUniqueObjects,
                    ExceptionMessage = $"Image worker with the same URL {imageWorkerServiceUrl} already exist"
                }.ConvertToWebFaultException(HttpStatusCode.InternalServerError);
            }
        }

        public void UpdateImageWorker(string name, string newName, string newWorkerServiceUrl)
        {
            var worker = workerRepository.Find(item => item.Name == name).FirstOrDefault();

            if (worker == null)
            {
                throw new ErrorMessage()
                {
                    Code = ErrorMessage.ErorCode.ObjectNotFoundInDb,
                    ExceptionMessage = $"Image worker with name {name} not found"
                }.ConvertToWebFaultException(HttpStatusCode.InternalServerError);
            }
            else
            {
                worker.Name = newName;
                worker.WorkerServiceUrl = newWorkerServiceUrl;

                workerRepository.UpdateAndFlashChanges(worker);
            }
        }

        #endregion

        #region Kepler Configs

        public string GetDiffImageSavingPath()
        {
            var diffImgPathToSaveProperty = KeplerSystemConfigRepository.Instance.Find("DiffImagePath");
            return diffImgPathToSaveProperty == null ? "" : diffImgPathToSaveProperty.Value;
        }

        public string GetPreviewSavingPath()
        {
            var previewPathToSaveProperty = KeplerSystemConfigRepository.Instance.Find("PreviewPath");
            return previewPathToSaveProperty == null ? "" : previewPathToSaveProperty.Value;
        }

        public void SetDiffImageSavingPath(string diffImageSavingPath)
        {
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

                KeplerSystemConfigRepository.Instance.Update(diffImgPathToSaveProperty);
                KeplerSystemConfigRepository.Instance.Update(previewPathProperty);
                KeplerSystemConfigRepository.Instance.FlushChanges();
            }

            BuildExecutor.DiffImageSavingPath = diffImageSavingPath;
            BuildExecutor.PreviewImageSavingPath = previewPath;

            BuildExecutor.GetExecutor().UpdateKeplerServiceUrlOnWorkers();
            BuildExecutor.GetExecutor().UpdateDiffImagePath();
        }

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
            var error = new ErrorMessage()
            {
                Code = errorCode,
                ExceptionMessage = exceptionMessage
            };
            LogError(error);

            throw error.ConvertToWebFaultException(HttpStatusCode.InternalServerError);
        }

        public void LogError(ErrorMessage error)
        {
            ErrorMessageRepository.Instance.Insert(error);
        }

        public void SetLastViewedError(long errorId)
        {
            var error = ErrorMessageRepository.Instance.Get(errorId);
            if (error == null)
                throw new ErrorMessage()
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