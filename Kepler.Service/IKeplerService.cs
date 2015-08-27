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
    }
}