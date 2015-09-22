using System.Diagnostics;
using Kepler.Common.CommunicationTypes;
using KeplerImageProcessorService.TaskManager;
using Newtonsoft.Json;

namespace KeplerImageProcessorService
{
    public class KeplerImageProcessorService : IKeplerImageProcessorService
    {
        public int GetMaxCountWorkers()
        {
            return TaskGenerator.GetMaxCountWorkers();
        }

        public void AddImagesForDiffGeneration(string jsonImagesToProcess)
        {
            var transferMessage = JsonConvert.DeserializeObject<ImageComparisonMessage>(jsonImagesToProcess);

            EventLog.WriteEntry("image processor test", string.Join(", ", transferMessage.ImageComparisonList));

            //   TaskGenerator.GetTaskGenerator.AddImagesToProcess(transferMessage.ImageComparisonList);
        }
    }
}