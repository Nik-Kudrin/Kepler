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
using Kepler.Common.Util;
using Kepler.Service.Config;
using Kepler.Service.Core;
using Kepler.Service.RestWorkerClient;
using Kepler.Service.Scheduler;

namespace Kepler.Service
{
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    [ServiceBehavior(IncludeExceptionDetailInFaults = true, ConcurrencyMode = ConcurrencyMode.Multiple,
        InstanceContextMode = InstanceContextMode.Single)]
    public class KeplerService : IKeplerService
    {
        // do not remove this field (used for build executor and data clean scheduler init)
        private static BuildExecutor _executor = BuildExecutor.GetExecutor();
        private static CleanDataSchedulerExecutor _dataCleanExecutor = CleanDataSchedulerExecutor.GetExecutor();

        public void ImportTestConfig(string testConfig)
        {
            var configImporter = new ConfigImporter();
            configImporter.ImportConfig(testConfig);
        }

        public void UpdateScreenShots(ImageComparisonContract imageComparisonContract)
        {
            foreach (var imageComparisonInfo in imageComparisonContract.ImageComparisonList)
            {
                var screenShotRepo = ScreenShotRepository.Instance;

                var screenShot = screenShotRepo.Get(imageComparisonInfo.ScreenShotId);

                // if current screenshot status = Stopped, then just update diff image path field
                if (screenShot.Status == ObjectStatus.Stopped)
                {
                    screenShot.PreviewImagePath = imageComparisonInfo.SecondPreviewPath;
                    screenShot.BaseLinePreviewPath = imageComparisonInfo.FirstPreviewPath;

                    // Generate Url paths
                    UrlPathGenerator.ReplaceFilePathWithUrl(screenShot);
                    screenShotRepo.Update(screenShot);
                    continue;
                }

                // if Failed
                if (imageComparisonInfo.IsImagesDifferent || imageComparisonInfo.ErrorMessage != "")
                {
                    screenShot.Status = ObjectStatus.Failed;
                    screenShot.ErrorMessage = imageComparisonInfo.ErrorMessage;
                }
                else // if Passed
                {
                    if (imageComparisonInfo.LastPassedScreenShotId.HasValue)
                    {
                        var oldPassedScreenShot =
                            screenShotRepo.Get(imageComparisonInfo.LastPassedScreenShotId.Value);
                        oldPassedScreenShot.IsLastPassed = false;
                        screenShotRepo.Update(oldPassedScreenShot);
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

                screenShotRepo.Update(screenShot);
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

                        if (typeName == "build") return;

                        long? buildId = -1;
                        switch (typeName.ToLowerInvariant())
                        {
                            case "screenshot":
                                var screenShot = ScreenShotRepository.Instance.Get(objId);
                                buildId = screenShot.BuildId;

                                var caseRepo = TestCaseRepository.Instance;
                                ObjectStatusUpdater.SetParentObjStatus<TestCaseRepository, TestCase>(
                                    caseRepo, screenShot.ParentObjId.Value, ObjectStatus.InQueue);
                                var suiteId = caseRepo.Get(screenShot.ParentObjId.Value).ParentObjId.Value;

                                var repoSuite = TestSuiteRepository.Instance;
                                ObjectStatusUpdater.SetParentObjStatus<TestSuiteRepository, TestSuite>(
                                    repoSuite, suiteId, ObjectStatus.InQueue);
                                var assembId = repoSuite.Get(suiteId).ParentObjId.Value;

                                ObjectStatusUpdater.SetParentObjStatus<TestAssemblyRepository, TestAssembly>(
                                    TestAssemblyRepository.Instance, assembId, ObjectStatus.InQueue);

                                break;
                            case "testcase":
                                var testCase = TestCaseRepository.Instance.Get(objId);
                                buildId = testCase.BuildId;

                                var suiteRepo = TestSuiteRepository.Instance;
                                ObjectStatusUpdater.SetParentObjStatus<TestSuiteRepository, TestSuite>(
                                    suiteRepo, testCase.ParentObjId.Value, ObjectStatus.InQueue);
                                var assemblyId = suiteRepo.Get(testCase.ParentObjId.Value).ParentObjId.Value;

                                ObjectStatusUpdater.SetParentObjStatus<TestAssemblyRepository, TestAssembly>(
                                    TestAssemblyRepository.Instance, assemblyId, ObjectStatus.InQueue);

                                break;
                            case "testsuite":
                                var suite = TestSuiteRepository.Instance.Get(objId);
                                buildId = suite.BuildId;

                                ObjectStatusUpdater.SetParentObjStatus<TestAssemblyRepository, TestAssembly>(
                                    TestAssemblyRepository.Instance, suite.ParentObjId.Value, ObjectStatus.InQueue);

                                break;
                            case "testassembly":
                                buildId = TestAssemblyRepository.Instance.Get(objId).BuildId;
                                break;
                        }

                        var buildRepo = BuildRepository.Instance;

                        var build = buildRepo.Get(buildId.Value);
                        build.Status = ObjectStatus.InQueue;
                        buildRepo.Update(build);
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

            if (!string.IsNullOrEmpty(schedulerProperty))
                return new RestSharpDataContractJsonDeserializer().Deserialize<DataSchedulerContract>(schedulerProperty);

            // if scheduler is'n initialized (first time, when applications start)
            var scheduler = new DataSchedulerContract()
            {
                Name = schedulerName,
                SchedulePeriod = TimeSpan.FromDays(30),
                NextStartTime = DateTime.Now,
                HistoryItemsNumberToPreserve = 3
            };

            var serializer = new RestSharpDataContractJsonSerializer();
            var serializedProperty = serializer.Serialize(scheduler);
            SetKeplerConfigProperty(schedulerName, serializedProperty);

            return scheduler;
        }


        public void UpdateCleanDataScheduler(DataSchedulerContract scheduler)
        {
            if (WebOperationContext.Current != null && WebOperationContext.Current.IncomingRequest.Method == "OPTIONS")
            {
                WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.OK;
                return;
            }

            if (scheduler.SchedulePeriod < TimeSpan.FromMinutes(20))
                LogErrorMessage(ErrorMessage.ErorCode.SetObjectStatusError,
                    $"Period for data cleaning must be more then 10 min");

            var serializer = new RestSharpDataContractJsonSerializer();
            var serializedProperty = serializer.Serialize(scheduler);
            SetKeplerConfigProperty(scheduler.Name, serializedProperty);
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
            return BuildRepository.Instance.Find(new {BranchId = branchId});
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
            return ScreenShotRepository.Instance.Find(new {ParentObjId = testCaseId});
        }

        #endregion

        #region TestCase

        public TestCase GetTestCase(long id)
        {
            var repo = new RepositoriesContainer();
            return repo.CaseRepo.GetCompleteObject(repo, id);
        }

        public IEnumerable<TestCase> GetTestCases(long testSuiteId)
        {
            var repo = new RepositoriesContainer();

            var cases = repo.CaseRepo.FindByParentId(testSuiteId).ToList();
            cases.ForEach(item => item.InitChildObjectsFromDb(repo));

            return cases;
        }

        #endregion

        #region TestSuite

        public TestSuite GetTestSuite(long id)
        {
            var repo = new RepositoriesContainer();
            return repo.SuiteRepo.GetCompleteObject(repo, id);
        }

        public IEnumerable<TestSuite> GetTestSuites(long assemblyId)
        {
            var repo = new RepositoriesContainer();

            var suites = repo.SuiteRepo.FindByParentId(assemblyId).ToList();
            suites.ForEach(item => item.InitChildObjectsFromDb(repo));

            return suites;
        }

        #endregion

        #region TestAssembly

        public TestAssembly GetTestAssembly(long id)
        {
            var repo = new RepositoriesContainer();
            return repo.AssemblyRepo.GetCompleteObject(repo, id);
        }


        public IEnumerable<TestAssembly> GetTestAssemblies(long buildId)
        {
            var repo = new RepositoriesContainer();

            var assemblies = repo.AssemblyRepo.FindByBuildId(buildId).ToList();
            assemblies.ForEach(item => item.InitChildObjectsFromDb(repo));

            return assemblies;
        }

        #endregion

        #region Project

        public Project GetProject(long id)
        {
            return ProjectRepository.Instance.GetCompleteObject(id);
        }

        public Project GetProjectByName(string name)
        {
            var projectId = ProjectRepository.Instance.Find(new { Name = name }).FirstOrDefault().Id;
            return ProjectRepository.Instance.GetCompleteObject(projectId);
        }


        public IEnumerable<Project> GetProjects()
        {
            var projectRepo = ProjectRepository.Instance;
            var projects = projectRepo.FindAll();
            projects.Each(project => project.InitChildObjectsFromDb());

            projects = projects.OrderBy(item => item.Name, StringComparer.InvariantCultureIgnoreCase);

            foreach (var project in projects)
            {
                project.Branches = project.Branches
                    .OrderBy(branch => branch.Value.Name, StringComparer.InvariantCultureIgnoreCase)
                    .ToDictionary(item => item.Key, item => item.Value);
            }

            return projects;
        }

        public void CreateProject(string name)
        {
            var projectRepo = ProjectRepository.Instance;
            if (projectRepo.Find(new {Name = name}).Any())
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
            var projectRepo = ProjectRepository.Instance;

            var project = projectRepo.Get(id);

            if (project == null)
            {
                LogErrorMessage(ErrorMessage.ErorCode.ObjectNotFoundInDb, $"Project with Id={id} doesn't exist");
            }

            if (project.Name != newName)
            {
                if (projectRepo.Find(new {Name = newName}).Any())
                {
                    LogErrorMessage(ErrorMessage.ErorCode.NotUniqueObjects, $"Project with name {newName} already exist");
                }
            }

            project.Name = newName;
            projectRepo.Update(project);
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
                baseline.Id = BaseLineRepository.Instance.Insert(baseline).Value;

                var branch = new Branch
                {
                    Name = name,
                    BaseLineId = baseline.Id,
                    ProjectId = projectId
                };
                branch.Id = BranchRepository.Instance.Insert(branch).Value;

                baseline.BranchId = branch.Id;
                BaseLineRepository.Instance.Update(baseline);

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
            IsBranchAlreadyExist(branchName, projectId);

            var project = ProjectRepository.Instance.Get(projectId);
            if (project == null)
            {
                LogErrorMessage(ErrorMessage.ErorCode.ObjectNotFoundInDb,
                    $"Project with specified projectID {projectId} doesn't exist");
            }


            if (!project.MainBranchId.HasValue &&
                BranchRepository.Instance.Find(new {ProjectId = project.Id}).Any())
            {
                LogErrorMessage(ErrorMessage.ErorCode.ProjectDontHaveMainBranch,
                    $"Project '{project.Name}' doesn't have main branch. Please, manually specify for project, which branch should be considered as main.");
            }
        }

        private void IsBranchAlreadyExist(string branchName, long projectId)
        {
            if (BranchRepository.Instance.Find(new {Name = branchName, ProjectId = projectId}).Any())
            {
                LogErrorMessage(ErrorMessage.ErorCode.NotUniqueObjects, $"Branch with name '{branchName}' already exist");
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
                IsBranchAlreadyExist(newName, branch.ProjectId.Value);
            }

            if (isMainBranch)
            {
                var allProjectBranches =
                    BranchRepository.Instance.Find(new {ProjectId = branch.ProjectId}).ToList();

                allProjectBranches.ForEach(item => item.IsMainBranch = false);
                BranchRepository.Instance.Update(allProjectBranches);

                var project = ProjectRepository.Instance.Get(branch.ProjectId.Value);
                project.MainBranchId = branch.Id;
                ProjectRepository.Instance.Update(project);
            }

            branch.Name = newName;
            branch.IsMainBranch = isMainBranch;
            BranchRepository.Instance.Update(branch);
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
            var branches = BranchRepository.Instance.Find(new {ProjectId = projectId});
            branches.Each(branch => branch.InitChildObjectsFromDb<BuildRepository, Build>(BuildRepository.Instance));

            return branches.OrderBy(item => item.Name, StringComparer.InvariantCultureIgnoreCase);
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
            imageWorkerServiceUrl = imageWorkerServiceUrl.Trim();

            var workerRepo = ImageWorkerRepository.Instance;
            if (!workerRepo.Find(new {Name = imageWorkerServiceUrl}).Any())
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
                    $"Image worker with the same URL {imageWorkerServiceUrl} already exist");
            }
        }

        public void UpdateImageWorker(string name, string newName, string newWorkerServiceUrl)
        {
            var workerRepo = ImageWorkerRepository.Instance;
            var worker = workerRepo.Find(new {Name = name}).FirstOrDefault();

            if (worker == null)
            {
                LogErrorMessage(ErrorMessage.ErorCode.ObjectNotFoundInDb, $"Image worker with name {name} not found");
            }
            else
            {
                worker.Name = newName;
                worker.WorkerServiceUrl = newWorkerServiceUrl;
                workerRepo.Update(worker);
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
                LogErrorMessage(ErrorMessage.ErorCode.UndefinedError, "Kepler config property name must be not emtpy");

            var property = KeplerSystemConfigRepository.Instance.Find(new {Name = propertyName}).FirstOrDefault();
            return property == null ? "" : property.Value;
        }

        private void SetKeplerConfigProperty(string propertyName, string propertyValue)
        {
            var property = KeplerSystemConfigRepository.Instance.Find(new {Name = propertyName}).FirstOrDefault();

            if (property == null)
            {
                KeplerSystemConfigRepository.Instance.Insert(new KeplerSystemConfig(propertyName, propertyValue));
            }
            else
            {
                property.Value = propertyValue;
                KeplerSystemConfigRepository.Instance.Update(property);
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
            return ErrorMessageRepository.Instance.Find(string.Format("WHERE Time >= '{0}'",
                fromTime.ToString("yyyy-MM-dd HH:mm:ss")))
                .OrderByDescending(item => item.Id);
        }

        public IEnumerable<ErrorMessage> GetErrorsSinceLastViewed()
        {
            var errorRepo = ErrorMessageRepository.Instance;

            var lastViewedError = errorRepo.Find(new {IsLastViewed = true}).FirstOrDefault();

            if (lastViewedError == null)
            {
                return errorRepo.FindAll().OrderByDescending(item => item.Id);
            }

            return errorRepo.Find(string.Format("WHERE Id > {0}", lastViewedError.Id))
                .OrderByDescending(item => item.Id);
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


            var allLastViewedItems = ErrorMessageRepository.Instance.Find(new {IsLastViewed = true});
            allLastViewedItems.Each(item => item.IsLastViewed = false);
            ErrorMessageRepository.Instance.Update(allLastViewedItems);

            error.IsLastViewed = true;
            ErrorMessageRepository.Instance.Update(error);
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

        #endregion
    }
}