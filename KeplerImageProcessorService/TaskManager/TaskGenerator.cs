using System.Collections.Generic;
using System.Linq;
using Kepler.Common.Models;

namespace KeplerImageProcessorService.TaskManager
{
    public class TaskGenerator
    {
        private static TaskGenerator _taskGenerator;
        private List<ImageTaskWorker> _taskWorkers = new List<ImageTaskWorker>();
        public static int MaxCountWorkers;

        static TaskGenerator()
        {
            MaxCountWorkers = 1; // TODO: license should be placed here
        }

        public static TaskGenerator TaskContainer
        {
            get { return _taskGenerator ?? new TaskGenerator(); }
        }

        private void InitTaskWorkers()
        {
            for (int index = 0; index < MaxCountWorkers; index++)
            {
                _taskWorkers.Add(new ImageTaskWorker());
            }
        }

        public void AddImagesToProcess(List<ImageInfo> images)
        {
            var imagesPerWorker = images.Count()/MaxCountWorkers;
            if (imagesPerWorker == 0)
                imagesPerWorker = images.Count();

            int workerIndex = 0;
            while (images.Count > 0)
            {
                _taskWorkers[workerIndex++].AddImagesForProcessing(images.Take(imagesPerWorker));
                images.RemoveRange(0, imagesPerWorker);

                if (workerIndex >= MaxCountWorkers)
                    workerIndex = 0;
            }
        }

        public IEnumerable<ImageInfo> GetProcessImages()
        {
        }
    }
}