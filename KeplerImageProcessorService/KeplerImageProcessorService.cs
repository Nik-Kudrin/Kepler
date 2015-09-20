using System;
using KeplerImageProcessorService.TaskManager;

namespace KeplerImageProcessorService
{
    public class KeplerImageProcessorService : IKeplerImageProcessorService
    {
        public int GetMaxCountWorkers()
        {
            return TaskGenerator.GetMaxCountWorkers();
        }
    }
}