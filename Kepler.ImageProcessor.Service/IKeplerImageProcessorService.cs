﻿using System.ServiceModel;
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
            BodyStyle = WebMessageBodyStyle.Bare, UriTemplate = "/AddImagesForDiffGeneration")]
        string AddImagesForDiffGeneration(ImageComparisonContract imagesToProcess);
    }
}