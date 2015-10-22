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
        #region Common Methods

        /// <summary>
        /// Run / Stop operation recursively on Build, TestCase ...
        /// </summary>
        /// <param name="typeName">Possible values: build, testCase, testSuite, testAssembly, screenShot</param>
        /// <param name="operationName">Possible values: run, stop</param>
        /// <returns></returns>
        [OperationContract]
        [WebGet(ResponseFormat = WebMessageFormat.Json,
            UriTemplate = "RunOperation?typeName={typeName}&operationName={operationName}")
        ]
        string RunOperation(string typeName, string operationName);

        /// <summary>
        /// Set new status for objects recursively
        /// </summary>
        /// <param name="typeName">Possible values: build, testCase, testSuite, testAssembly, screenShot</param>
        /// <param name="status">Possible values: failed, passed</param>
        /// <returns></returns>
        [OperationContract]
        [WebGet(ResponseFormat = WebMessageFormat.Json,
            UriTemplate = "SetStatus?typeName={typeName}&status={status}")
        ]
        string SetStatus(string typeName, string status);

        #endregion

        [OperationContract]
        [WebGet(ResponseFormat = WebMessageFormat.Json, UriTemplate = "GetBuild?id={id}")]
        Build GetBuild(string id);

        [OperationContract]
        [WebGet(ResponseFormat = WebMessageFormat.Json, UriTemplate = "GetBuilds")]
        IEnumerable<Build> GetBuilds();

        #region TestCase

        [OperationContract]
        [WebGet(ResponseFormat = WebMessageFormat.Json, UriTemplate = "GetTestCase?testCaseId={id}")]
        TestCase GetTestCase(string id);

        [OperationContract]
        [WebGet(ResponseFormat = WebMessageFormat.Json, UriTemplate = "GetTestCases?testSuiteId={testSuiteId}")]
        IEnumerable<TestCase> GetTestCases(string testSuiteId);

        #endregion

        #region TestSuite

        [OperationContract]
        [WebGet(ResponseFormat = WebMessageFormat.Json, UriTemplate = "GetTestSuite?id={id}")]
        TestSuite GetTestSuite(string id);

        [OperationContract]
        [WebGet(ResponseFormat = WebMessageFormat.Json, UriTemplate = "GetTestSuites?assemblyId={assemblyId}")]
        IEnumerable<TestSuite> GetTestSuites(string assemblyId);

        #endregion

        #region TestAssembly

        [OperationContract]
        [WebGet(ResponseFormat = WebMessageFormat.Json, UriTemplate = "GetTestAssembly?assemblyId={assemblyId}")]
        TestAssembly GetTestAssembly(string assemblyId);

        [OperationContract]
        [WebGet(ResponseFormat = WebMessageFormat.Json, UriTemplate = "GetTestAssemblies?buildId={buildId}")]
        IEnumerable<TestAssembly> GetTestAssemblies(string buildId);

        #endregion

        #region Project

        [OperationContract]
        [WebGet(ResponseFormat = WebMessageFormat.Json, UriTemplate = "GetProject?projectId={id}")]
        Project GetProject(string id);

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
        string CreateProject(string name);

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
        string CreateBranch(string name, long projectId);

        [OperationContract]
        [WebGet(RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare,
            UriTemplate = "UpdateBranch?name={name}&newName={newName}&isMainBranch={isMainBranch}")]
        string UpdateBranch(string name, string newName, bool isMainBranch);

        #endregion

        /// <summary>
        /// Import test config
        /// </summary>
        /// <param name="testConfig"></param>
        /// <returns>Return emtpy string, if import was OK. Otherwise return string with error message</returns>
        [OperationContract]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare, UriTemplate = "ImportTestConfig")]
        string ImportTestConfig(string testConfig);


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
        string RegisterImageWorker(string name, string imageWorkerServiceUrl);

        [OperationContract]
        [WebGet(RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare,
            UriTemplate = "UpdateImageWorker?name={name}&newName={newName}&newWorkerServiceUrl={newWorkerServiceUrl}")]
        string UpdateImageWorker(string name, string newName, string newWorkerServiceUrl);

        #endregion

        #region Project

        [OperationContract]
        [WebGet(ResponseFormat = WebMessageFormat.Json, UriTemplate = "GetDiffImageSavingPath")]
        string GetDiffImageSavingPath();

        [OperationContract]
        [WebGet(RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare,
            UriTemplate = "SetDiffImageSavingPath?diffImageSavingPath={diffImageSavingPath}")]
        void SetDiffImageSavingPath(string diffImageSavingPath);

        #endregion
    }
}