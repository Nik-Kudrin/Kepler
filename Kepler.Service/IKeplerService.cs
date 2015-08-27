using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Web;
using Kepler.Core;

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
        IEnumerable<Build> GetTestCase(string id);


        [OperationContract]
        [WebGet(ResponseFormat = WebMessageFormat.Json, UriTemplate = "GetTestCases?testSuiteId={testSuiteId}")]
        IEnumerable<Build> GetTestCases(string testSuiteId);

        #endregion

        #region TestSuite

        [OperationContract]
        [WebGet(ResponseFormat = WebMessageFormat.Json, UriTemplate = "GetTestSuite?id={id}")]
        IEnumerable<Build> GetTestSuite(string id);


        [OperationContract]
        [WebGet(ResponseFormat = WebMessageFormat.Json, UriTemplate = "GetTestSuites?assemblyId={assemblyId}")]
        IEnumerable<Build> GetTestSuites(string assemblyId);

        #endregion

        #region TestAssembly

        [OperationContract]
        [WebGet(ResponseFormat = WebMessageFormat.Json, UriTemplate = "GetTestAssembly?assemblyId={id}")]
        IEnumerable<Build> GetTestAssembly(string assemblyId);


        [OperationContract]
        [WebGet(ResponseFormat = WebMessageFormat.Json, UriTemplate = "GetTestAssemblies?buildId={buildId}")]
        IEnumerable<Build> GetTestAssemblies(string buildId);

        #endregion

        #region Project

        [OperationContract]
        [WebGet(ResponseFormat = WebMessageFormat.Json, UriTemplate = "GetProject?projectId={id}")]
        IEnumerable<Build> GetProject(string id);

        [OperationContract]
        [WebGet(ResponseFormat = WebMessageFormat.Json, UriTemplate = "GetProjects")]
        IEnumerable<Build> GetProjects();

        #endregion

        #region Baseline

        [OperationContract]
        [WebGet(ResponseFormat = WebMessageFormat.Json, UriTemplate = "GetBaseline?baseLineId={id}")]
        IEnumerable<Build> GetBaseline(string id);

        #endregion
    }
}