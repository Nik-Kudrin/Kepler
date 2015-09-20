using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Timers;
using Kepler.Common.Models;

namespace KeplerImageProcessorService.TaskManager
{
    public class TaskGenerator
    {
        private static TaskGenerator _taskGenerator;
        private List<ImageTaskWorker> _taskWorkers = new List<ImageTaskWorker>();
        public static int MaxCountWorkers;
        private Timer _timer;


        static TaskGenerator()
        {
            MaxCountWorkers = 1; // TODO: license should be placed here
        }

        public static TaskGenerator TaskContainer
        {
            get { return _taskGenerator ?? new TaskGenerator(); }
        }


        private TaskGenerator()
        {
            InitTaskWorkers();

            _timer = new Timer();
            _timer.Interval = 10000; //every 10sec
            _timer.Elapsed += new ElapsedEventHandler(this.RunWorkers);
            _timer.Elapsed += new ElapsedEventHandler(this.SendProcessedImagesToKeplerService);
            _timer.AutoReset = true;

            _timer.Enabled = true;
        }

        private void InitTaskWorkers()
        {
            for (int index = 0; index < MaxCountWorkers; index++)
            {
                _taskWorkers.Add(new ImageTaskWorker());
            }
        }


        private void RunWorkers(Object sender, ElapsedEventArgs eventArgs)
        {
            for (int index = 0; index < MaxCountWorkers; index++)
            {
                var currentWorker = _taskWorkers[index];

                if (currentWorker.AssignedTask == null || currentWorker.AssignedTask.IsCompleted)
                {
                    var task = Task.Factory.StartNew(currentWorker.ProcessImages);
                    currentWorker.AssignedTask = task;
                    task.Start();
                }
                else if (currentWorker.AssignedTask.IsFaulted || currentWorker.AssignedTask.IsCanceled)
                {
                    currentWorker.AssignedTask = null;
                }
            }
        }

        // TODO: Tasks should be run always. And go sleep, when list of image for processing is empty

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

        public IEnumerable<ImageInfo> GetProcessedImages()
        {
            var processedImages = new List<ImageInfo>();

            foreach (var imageTaskWorker in _taskWorkers)
            {
                processedImages.AddRange(imageTaskWorker.GetProcessedImages());
            }

            return processedImages;
        }

        private void SendProcessedImagesToKeplerService(Object sender, ElapsedEventArgs eventArgs)
        {
            // TODO: write...
            Console.WriteLine(" >>> Stub . Send processed images to kepler service");
            // If there are no one processed images - do not call rest service
            throw new NotImplementedException();
        }
    }
}