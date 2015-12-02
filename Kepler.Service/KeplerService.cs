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
        private readonly TestAssemblyRepository assemblyRepository = TestAssemblyRepository.Instance;
        private readonly BuildRepository buildRepo = BuildRepository.Instance;
        private readonly ProjectRepository projectRepository = ProjectRepository.Instance;
        private readonly TestCaseRepository testCaseRepo = TestCaseRepository.Instance;
        private readonly TestSuiteRepository testSuiteRepo = TestSuiteRepository.Instance;
        private readonly ImageWorkerRepository workerRepository = ImageWorkerRepository.Instance;


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
                        LogErrorMessage(ErrorMessage.ErorCode.RunOperationError, ex.Message);
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
                LogErrorMessage(ErrorMessage.ErorCode.SetObjectStatusError, ex.Message);
            }
        }

        #endregion

        #region Build

        public Build GetBuild(long id)
        {
            return buildRepo.Get(id);
        }

        public IEnumerable<Build> GetBuilds(long branchId)
        {
            return buildRepo.Find(item => item.BranchId == branchId);
        }

        public void DeleteBuild(long id)
        {
            var build = GetBuild(id);

            if (build == null)
                LogErrorMessage(ErrorMessage.ErorCode.ObjectNotFoundInDb, $"Build with ID {id} was not found");

            RunOperation("build", id, "stop");

            // TODO: delete test assemblies, suites, cases, screenshots
            // TODO: delete build directory (preview and diff)

            throw new NotImplementedException();
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
                LogErrorMessage(ErrorMessage.ErorCode.NotUniqueObjects, $"Project with name {name} already exist");

            try
            {
                var project = new Project {Name = name};
                projectRepository.Insert(project);
            }
            catch (Exception ex)
            {
                LogErrorMessage(ErrorMessage.ErorCode.UndefinedError, $"{ex.Message} {ex.StackTrace}");
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
            throw new NotImplementedException();
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
                LogErrorMessage(ErrorMessage.ErorCode.UndefinedError, $"{ex.Message} {ex.StackTrace}");
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
            return BranchRepository.Instance.GetCompleteObject(id);
        }

        public IEnumerable<Branch> GetBranches(long projectId)
        {
            var branches = BranchRepository.Instance.Find(branch => branch.ProjectId == projectId);
            branches.Each(branch => branch.InitChildObjectsFromDb());

            return branches;
        }

        public void DeleteBranch(long id)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region ImageWorkers

        public IEnumerable<ImageWorker> GetImageWorkers()
        {
            return workerRepository.FindAll();
        }


        public void RegisterImageWorker(string name, string imageWorkerServiceUrl)
        {
            if (!workerRepository.Find(imageWorkerServiceUrl).Any())
            {
                workerRepository.Insert(new ImageWorker
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
            var worker = workerRepository.Find(item => item.Name == name).FirstOrDefault();

            if (worker == null)
            {
                LogErrorMessage(ErrorMessage.ErorCode.ObjectNotFoundInDb, $"Image worker with name {name} not found");
            }
            else
            {
                worker.Name = newName;
                worker.WorkerServiceUrl = newWorkerServiceUrl;
                workerRepository.UpdateAndFlashChanges(worker);
            }
        }

        public void DeleteImageWorker(long id)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Kepler Configs

        public string GetDiffImageSavingPath()
        {
            var property = KeplerSystemConfigRepository.Instance.Find("DiffImagePath");
            return property == null ? "" : property.Value;
        }

        public string GetPreviewSavingPath()
        {
            var previewPathToSaveProperty = KeplerSystemConfigRepository.Instance.Find("PreviewPath");
            return previewPathToSaveProperty == null ? "" : previewPathToSaveProperty.Value;
        }

        public void SetDiffImageSavingPath(string diffImageSavingPath)
        {
            diffImageSavingPath = diffImageSavingPath.ToLowerInvariant();
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

            BuildExecutor.GetExecutor().UpdateKeplerServiceUrlOnWorkers();
            BuildExecutor.GetExecutor().UpdateDiffImagePath();
            UrlPathGenerator.DiffImagePath = new KeplerService().GetDiffImageSavingPath();
        }

        public string GetSourceImagePath()
        {
            var property = KeplerSystemConfigRepository.Instance.Find("SourceImagePath");
            return property == null ? "" : property.Value;
        }

        public void SetSourceImageSavingPath(string sourceImageSavingPath)
        {
            sourceImageSavingPath = sourceImageSavingPath.ToLowerInvariant();
            var diffImgPathToSaveProperty = KeplerSystemConfigRepository.Instance.Find("SourceImagePath");

            if (diffImgPathToSaveProperty == null)
            {
                KeplerSystemConfigRepository.Instance.Insert(new KeplerSystemConfig("SourceImagePath",
                    sourceImageSavingPath));
            }
            else
            {
                diffImgPathToSaveProperty.Value = sourceImageSavingPath;
                KeplerSystemConfigRepository.Instance.Update(diffImgPathToSaveProperty);
                KeplerSystemConfigRepository.Instance.FlushChanges();
            }

            UrlPathGenerator.SourceImagePath = new KeplerService().GetSourceImagePath();
        }

        public string GetKeplerServiceUrl()
        {
            var property = KeplerSystemConfigRepository.Instance.Find("KeplerServiceUrl");
            return property == null ? "" : property.Value;
        }

        public void SetKeplerServiceUrl(string url)
        {
            var keplerServiceUrlProperty = KeplerSystemConfigRepository.Instance.Find("KeplerServiceUrl");

            if (keplerServiceUrlProperty == null)
            {
                KeplerSystemConfigRepository.Instance.Insert(new KeplerSystemConfig("KeplerServiceUrl", url));
            }
            else
            {
                keplerServiceUrlProperty.Value = url;
                KeplerSystemConfigRepository.Instance.Update(keplerServiceUrlProperty);
                KeplerSystemConfigRepository.Instance.FlushChanges();
            }

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