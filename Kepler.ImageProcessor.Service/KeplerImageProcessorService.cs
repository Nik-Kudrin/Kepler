using System;
using System.Net;
using System.ServiceModel;
using System.ServiceModel.Activation;
using Kepler.Common.CommunicationContracts;
using Kepler.Common.Error;
using Kepler.ImageProcessor.Service.TaskManager;

namespace Kepler.ImageProcessor.Service
{
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    [ServiceBehavior(IncludeExceptionDetailInFaults = true)]
    public class KeplerImageProcessorService : IKeplerImageProcessorService
    {
        public int GetMaxCountWorkers()
        {
            return TaskGenerator.GetMaxCountWorkers();
        }

        public void AddImagesForDiffGeneration(ImageComparisonContract imagesToProcess)
        {
            try
            {
                TaskGenerator.GetTaskGenerator.AddImagesForProcessing(imagesToProcess.ImageComparisonList);
            }
            catch (Exception ex)
            {
                throw new ErrorMessage()
                {
                    Code = ErrorMessage.ErorCode.AddTaskToImageWorkerError,
                    ExceptionMessage = ex.Message
                }.ConvertToWebFaultException(HttpStatusCode.InternalServerError);
            }
        }

        public void SetKeplerServiceUrl(string url)
        {
            TaskGenerator.KeplerServiceUrl = url;
        }

        public void StopDiffGeneration(ImageComparisonContract imagesToStopProcessing)
        {
            TaskGenerator.GetTaskGenerator.RemoveImagesFromProcessing(imagesToStopProcessing.ImageComparisonList);
        }
    }
}