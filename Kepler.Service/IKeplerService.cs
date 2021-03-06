﻿using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Web;
using Kepler.Common.CommunicationContracts;
using Kepler.Common.Error;
using Kepler.Common.Models;

namespace Kepler.Service
{
    [ServiceContract]
    public interface IKeplerService
    {
        /// <summary>
        ///     Import test config
        /// </summary>
        /// <param name="testConfig"></param>
        /// <returns>Return emtpy string, if import was OK. Otherwise return string with error message</returns>
        [OperationContract]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare, UriTemplate = "ImportTestConfig")]
        void ImportTestConfig(string testConfig);


        [OperationContract]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare, UriTemplate = "UpdateScreenShots")]
        void UpdateScreenShots(ImageComparisonContract imageComparisonContract);

        #region Common Actions

        /// <summary>
        ///     Run / Stop operation recursively on Build, TestCase ...
        /// </summary>
        /// <param name="typeName">Possible values: build, testCase, testSuite, testAssembly, screenShot</param>
        /// <param name="objId"></param>
        /// <param name="operationName">Possible values: run, stop</param>
        /// <returns></returns>
        [OperationContract]
        [WebGet(ResponseFormat = WebMessageFormat.Json,
            UriTemplate = "RunOperation?typeName={typeName}&objId={objId}&operationName={operationName}")
        ]
        void RunOperation(string typeName, long objId, string operationName);

        /// <summary>
        ///     Set new newStatus for objects recursively
        /// </summary>
        /// <param name="typeName">Possible values: build, testCase, testSuite, testAssembly, screenShot</param>
        /// <param name="newStatus">Possible values: failed, passed</param>
        /// <returns></returns>
        [OperationContract]
        [WebGet(ResponseFormat = WebMessageFormat.Json,
            UriTemplate = "SetObjectsStatus?typeName={typeName}&objId={objId}&newStatus={newStatus}")
        ]
        void SetObjectsStatus(string typeName, long objId, string newStatus);

        #endregion

        #region Scheduler

        /// <summary>
        /// Get clean scheduler info
        /// </summary>
        /// <param name="schedulerName">Possible values: buildCleanScheduler, logCleanScheduler</param>
        /// <returns></returns>
        [OperationContract]
        [WebGet(ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare,
            UriTemplate = "GetCleanDataScheduler?schedulerName={schedulerName}"
            )
        ]
        DataSchedulerContract GetCleanDataScheduler(string schedulerName);

        /// <summary>
        /// Set new scheduler settings for data cleaning
        /// </summary>
        /// <param name="scheduler"></param>
        [OperationContract]
        [WebInvoke(Method = "*", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare, UriTemplate = "UpdateCleanDataScheduler")
        ]
        void UpdateCleanDataScheduler(DataSchedulerContract scheduler);

        #endregion

        #region Build

        [OperationContract]
        [WebGet(ResponseFormat = WebMessageFormat.Json, UriTemplate = "GetBuild?id={id}")]
        Build GetBuild(long id);

        [OperationContract]
        [WebGet(ResponseFormat = WebMessageFormat.Json, UriTemplate = "GetBuilds?branchId={branchId}")]
        IEnumerable<Build> GetBuilds(long branchId);

        [OperationContract]
        [WebGet(ResponseFormat = WebMessageFormat.Json, UriTemplate = "DeleteBuild?id={id}")]
        void DeleteBuild(long id);

        #endregion

        #region ScreenShot

        [OperationContract]
        [WebGet(ResponseFormat = WebMessageFormat.Json, UriTemplate = "GetScreenShot?id={id}")]
        ScreenShot GetScreenShot(long id);

        [OperationContract]
        [WebGet(ResponseFormat = WebMessageFormat.Json, UriTemplate = "GetScreenShots?testCaseId={testCaseId}")]
        IEnumerable<ScreenShot> GetScreenShots(long testCaseId);

        #endregion

        #region TestCase

        [OperationContract]
        [WebGet(ResponseFormat = WebMessageFormat.Json, UriTemplate = "GetTestCase?id={id}")]
        TestCase GetTestCase(long id);

        [OperationContract]
        [WebGet(ResponseFormat = WebMessageFormat.Json, UriTemplate = "GetTestCases?testSuiteId={testSuiteId}")]
        IEnumerable<TestCase> GetTestCases(long testSuiteId);

        #endregion

        #region TestSuite

        [OperationContract]
        [WebGet(ResponseFormat = WebMessageFormat.Json, UriTemplate = "GetTestSuite?id={id}")]
        TestSuite GetTestSuite(long id);

        [OperationContract]
        [WebGet(ResponseFormat = WebMessageFormat.Json, UriTemplate = "GetTestSuites?assemblyId={assemblyId}")]
        IEnumerable<TestSuite> GetTestSuites(long assemblyId);

        #endregion

        #region TestAssembly

        [OperationContract]
        [WebGet(ResponseFormat = WebMessageFormat.Json, UriTemplate = "GetTestAssembly?id={id}")]
        TestAssembly GetTestAssembly(long id);

        [OperationContract]
        [WebGet(ResponseFormat = WebMessageFormat.Json, UriTemplate = "GetTestAssemblies?buildId={buildId}")]
        IEnumerable<TestAssembly> GetTestAssemblies(long buildId);

        #endregion

        #region Project

        [OperationContract]
        [WebGet(ResponseFormat = WebMessageFormat.Json, UriTemplate = "GetProject?id={id}")]
        Project GetProject(long id);

        [OperationContract]
        [WebGet(ResponseFormat = WebMessageFormat.Json, UriTemplate = "GetProjectByName?name={name}")]
        Project GetProjectByName(string name);

        [OperationContract]
        [WebGet(ResponseFormat = WebMessageFormat.Json, UriTemplate = "GetProjects")]
        IEnumerable<Project> GetProjects();

        /// <summary>
        ///     Create project
        /// </summary>
        /// <param name="name"></param>
        /// <returns>Return empty string if operation was OK. Otherwis - error message</returns>
        [OperationContract]
        [WebGet(RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare, UriTemplate = "CreateProject?name={name}")]
        void CreateProject(string name);

        [OperationContract]
        [WebGet(RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare,
            UriTemplate = "UpdateProject?id={id}&newName={newName}")]
        void UpdateProject(long id, string newName);

        [OperationContract]
        [WebGet(ResponseFormat = WebMessageFormat.Json, UriTemplate = "DeleteProject?id={id}")]
        void DeleteProject(long id);

        #endregion

        #region Branch

        /// <summary>
        ///     Create Branch
        /// </summary>
        /// <param name="name"></param>
        /// <returns>Return empty string if operation was OK. Otherwis - error message</returns>
        [OperationContract]
        [WebGet(RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare, UriTemplate = "CreateBranch?name={name}&projectId={projectId}")]
        void CreateBranch(string name, long projectId);

        [OperationContract]
        [WebGet(RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare,
            UriTemplate = "UpdateBranch?id={id}&newName={newName}&isMainBranch={isMainBranch}")]
        void UpdateBranch(long id, string newName, bool isMainBranch);

        [OperationContract]
        [WebGet(RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare,
            UriTemplate = "GetBranch?id={id}")]
        Branch GetBranch(long id);

        [OperationContract]
        [WebGet(RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare,
            UriTemplate = "GetBranches?projectId={projectId}")]
        IEnumerable<Branch> GetBranches(long projectId);

        [OperationContract]
        [WebGet(ResponseFormat = WebMessageFormat.Json, UriTemplate = "DeleteBranch?id={id}")]
        void DeleteBranch(long id);

        #endregion

        #region ImageWorkers

        [OperationContract]
        [WebGet(ResponseFormat = WebMessageFormat.Json, UriTemplate = "GetImageWorkers")]
        IEnumerable<ImageWorker> GetImageWorkers();

        [OperationContract]
        [WebGet(RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare,
            UriTemplate = "RegisterImageWorker?name={name}&imageWorkerServiceUrl={imageWorkerServiceUrl}")]
        void RegisterImageWorker(string name, string imageWorkerServiceUrl);

        [OperationContract]
        [WebGet(RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare,
            UriTemplate = "UpdateImageWorker?name={name}&newName={newName}&newWorkerServiceUrl={newWorkerServiceUrl}")]
        void UpdateImageWorker(string name, string newName, string newWorkerServiceUrl);

        [OperationContract]
        [WebGet(ResponseFormat = WebMessageFormat.Json, UriTemplate = "DeleteImageWorker?id={id}")]
        void DeleteImageWorker(long id);

        #endregion

        #region Kepler Configs

        [OperationContract]
        [WebGet(ResponseFormat = WebMessageFormat.Json, UriTemplate = "GetDiffImageSavingPath")]
        string GetDiffImageSavingPath();

        [OperationContract]
        [WebGet(ResponseFormat = WebMessageFormat.Json, UriTemplate = "GetPreviewSavingPath")]
        string GetPreviewSavingPath();

        [OperationContract]
        [WebGet(RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare,
            UriTemplate = "SetDiffImageSavingPath?diffImageSavingPath={diffImageSavingPath}")]
        void SetDiffImageSavingPath(string diffImageSavingPath);

        [OperationContract]
        [WebGet(ResponseFormat = WebMessageFormat.Json, UriTemplate = "GetSourceImagePath")]
        string GetSourceImagePath();

        [OperationContract]
        [WebGet(RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare,
            UriTemplate = "SetSourceImageSavingPath?sourceImageSavingPath={sourceImageSavingPath}")]
        void SetSourceImageSavingPath(string sourceImageSavingPath);

        [OperationContract]
        [WebGet(ResponseFormat = WebMessageFormat.Json, UriTemplate = "GetKeplerServiceUrl")]
        string GetKeplerServiceUrl();

        [OperationContract]
        [WebGet(RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare,
            UriTemplate = "SetKeplerServiceUrl?url={url}")]
        void SetKeplerServiceUrl(string url);

        #endregion

        #region Errors Logging

        [OperationContract]
        [WebGet(RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare,
            UriTemplate = "GetErrors?fromTime={fromTime}")]
        IEnumerable<ErrorMessage> GetErrors(DateTime fromTime);

        [OperationContract]
        [WebGet(RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare,
            UriTemplate = "GetErrorsSinceLastViewed")]
        IEnumerable<ErrorMessage> GetErrorsSinceLastViewed();

        [OperationContract]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare, UriTemplate = "LogError")]
        void LogError(ErrorMessage error);

        [OperationContract]
        [WebGet(RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare,
            UriTemplate = "SetLastViewedError?errorId={errorId}")]
        void SetLastViewedError(long errorId);

        #endregion
    }
}