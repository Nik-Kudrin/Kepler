using System.ServiceModel;
using System.ServiceModel.Web;
using Kepler.Common.CommunicationContracts;

namespace Kepler.ImageProcessor.Service
{
    [ServiceContract]
    public interface IKeplerImageProcessorService
    {
        [OperationContract]
        [WebGet(ResponseFormat = WebMessageFormat.Json, UriTemplate = "/GetMaxCountWorkers")]
        int GetMaxCountWorkers();

        /// <summary>
        /// Add list of images for processing
        /// </summary>
        /// <param name="imagesToProcess"></param>
        /// <returns>Return empty string if everything is OK, otherwise - error message</returns>
        [OperationContract]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare, UriTemplate = "AddImagesForDiffGeneration")]
        void AddImagesForDiffGeneration(ImageComparisonContract imagesToProcess);

        [OperationContract]
        [WebGet(RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare, UriTemplate = "SetKeplerServiceUrl?url={url}")]
        void SetKeplerServiceUrl(string url);

        [OperationContract]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare, UriTemplate = "StopDiffGeneration")]
        void StopDiffGeneration(ImageComparisonContract imagesToStopProcessing);
    }
}