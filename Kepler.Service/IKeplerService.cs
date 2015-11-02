using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Web;
using Kepler.Common.CommunicationContracts;
using Kepler.Common.Models;

namespace Kepler.Service
{
    [ServiceContract]
    public interface IKeplerService
    {
        #region Common Actions

        /// <summary>
        /// Run / Stop operation recursively on Build, TestCase ...
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
        /// Set new newStatus for objects recursively
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

        #region Build

        [OperationContract]
        [WebGet(ResponseFormat = WebMessageFormat.Json, UriTemplate = "GetBuild?id={id}")]
        Build GetBuild(long id);

        [OperationContract]
        [WebGet(ResponseFormat = WebMessageFormat.Json, UriTemplate = "GetBuilds")]
        IEnumerable<Build> GetBuilds();

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
        [WebGet(ResponseFormat = WebMessageFormat.Json, UriTemplate = "GetProjects")]
        IEnumerable<Project> GetProjects();

        /// <summary>
        /// Create project
        /// </summary>
        /// <param name="name"></param>
        /// <returns>Return empty string if operation was OK. Otherwis - error message</returns>
        [OperationContract]
        [WebGet(RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare, UriTemplate = "CreateProject?name={name}")]
        void CreateProject(string name);

        #endregion

        #region Branch

        /// <summary>
        /// Create Branch
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
            UriTemplate = "UpdateBranch?name={name}&newName={newName}&isMainBranch={isMainBranch}")]
        void UpdateBranch(string name, string newName, bool isMainBranch);

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

        #endregion

        /// <summary>
        /// Import test config
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

        #endregion

        #region Project

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

        #endregion
    }
}