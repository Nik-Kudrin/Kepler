using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;

namespace KeplerImageProcessorService
{
    [ServiceContract]
    public interface IKeplerImageProcessorService
    {
        [OperationContract]
        [WebGet(ResponseFormat = WebMessageFormat.Json, UriTemplate = "GetMaxCountWorkers")]
        int GetMaxCountWorkers();
    }
}