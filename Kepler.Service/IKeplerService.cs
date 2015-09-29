using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Web;
using Kepler.Core;
using Kepler.Models;

namespace Kepler.Service
{
    [ServiceContract]
    public interface IKeplerService
    {
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
        /// <param name="projectName"></param>
        /// <returns>Return empty string if operation was OK. Otherwis - error message</returns>
        [OperationContract]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare, UriTemplate = "CreateProject")]
        string CreateProject(string projectName);

        #endregion

        #region Branch

        #endregion

        /// <summary>
        /// Import test config
        /// </summary>
        /// <param name="testConfig"></param>
        /// <returns>Return emtpy string, if import was OK. Otherwise return string with error message</returns>
        [OperationContract]
        [WebInvoke(ResponseFormat = WebMessageFormat.Json, UriTemplate = "ImportTestConfig")]
        string ImportTestConfig(string testConfig);


        [OperationContract]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare, UriTemplate = "RegisterImageWorker")]
        void RegisterImageWorker(string imageWorkerServiceUrl);

        #region Project

        [OperationContract]
        [WebGet(ResponseFormat = WebMessageFormat.Json, UriTemplate = "GetDiffImageSavingPath")]
        string GetDiffImageSavingPath();

        [OperationContract]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare, UriTemplate = "SetDiffImageSavingPath")]
        void SetDiffImageSavingPath(string diffImageSavingPath);

        #endregion
    }
}