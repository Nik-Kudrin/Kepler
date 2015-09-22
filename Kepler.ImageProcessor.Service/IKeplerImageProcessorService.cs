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

        [OperationContract]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json,
            UriTemplate = "AddImagesForDiffGeneration?jsonImagesToProcess={jsonImagesToProcess}")]
        void AddImagesForDiffGeneration(string jsonImagesToProcess);
    }
}