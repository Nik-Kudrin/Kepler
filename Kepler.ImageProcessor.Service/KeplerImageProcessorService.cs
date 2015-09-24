using System;
using Kepler.Common.CommunicationContracts;
using Kepler.Common.Error;
using Kepler.ImageProcessor.Service.TaskManager;

namespace Kepler.ImageProcessor.Service
{
    public class KeplerImageProcessorService : IKeplerImageProcessorService
    {
        public int GetMaxCountWorkers()
        {
            return TaskGenerator.GetMaxCountWorkers();
        }

        public string AddImagesForDiffGeneration(ImageComparisonContract imagesToProcess)
        {
            try
            {
                TaskGenerator.GetTaskGenerator.AddImagesForProcessing(imagesToProcess.ImageComparisonList);
            }
            catch (Exception ex)
            {
                return new ErrorMessage() {Code = ErrorMessage.ErorCode.AddTaskToImageWorkerError, ExceptionMessage = ex.Message}.ToString();
            }

            return "";
        }
    }
}