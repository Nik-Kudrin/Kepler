using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Timers;
using Kepler.Common.CommunicationContracts;
using RestSharp;
using Timer = System.Timers.Timer;

namespace Kepler.ImageProcessor.Service.TaskManager
{
    public class TaskGenerator
    {
        private static string KeplerServiceUrl;
        private static TaskGenerator _taskGenerator;
        private List<ImageTaskWorker> _taskWorkers = new List<ImageTaskWorker>();
        private static int MaxCountWorkers;
        private Timer _timer;

        static TaskGenerator()
        {
            MaxCountWorkers = 1; // TODO: license should be placed here
        }

        public static TaskGenerator GetTaskGenerator
        {
            get
            {
                _taskGenerator = _taskGenerator ?? new TaskGenerator();
                return _taskGenerator;
            }
        }

        public static int GetMaxCountWorkers()
        {
            return MaxCountWorkers;
        }

        public void SetKeplerServiceUrl(string url)
        {
            KeplerServiceUrl = url;
        }

        private TaskGenerator()
        {
            InitTaskWorkers();

            _timer = new Timer();
            _timer.Interval = 10000; //every 10 sec
            _timer.Elapsed += RunWorkers;
            _timer.Elapsed += SendProcessedImagesToKeplerService;
            _timer.Enabled = true;
        }

        private void InitTaskWorkers()
        {
            for (int index = 0; index < MaxCountWorkers; index++)
            {
                _taskWorkers.Add(new ImageTaskWorker());
            }
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        private void RunWorkers(object sender, ElapsedEventArgs eventArgs)
        {
            for (int index = 0; index < MaxCountWorkers; index++)
            {
                var currentWorker = _taskWorkers[index];

                if (currentWorker.AssignedTask == null || currentWorker.AssignedTask.IsCompleted)
                {
                    var task = Task.Factory.StartNew(currentWorker.ProcessImages);
                    currentWorker.AssignedTask = task;
                }
                else if (currentWorker.AssignedTask.IsFaulted || currentWorker.AssignedTask.IsCanceled)
                {
                    currentWorker.AssignedTask = null;
                }
            }
        }


        public void AddImagesForProcessing(List<ImageComparisonInfo> images)
        {
            var imagesPerWorker = images.Count()/MaxCountWorkers;
            if (imagesPerWorker == 0)
                imagesPerWorker = images.Count();

            int workerIndex = 0;
            while (images.Count > 0)
            {
                _taskWorkers[workerIndex++].AddImagesForProcessing(images.Take(imagesPerWorker));

                if (imagesPerWorker < images.Count)
                    images.RemoveRange(0, imagesPerWorker);
                else
                {
                    images.Clear();
                }

                if (workerIndex >= MaxCountWorkers)
                    workerIndex = 0;
            }
        }

        public IEnumerable<ImageComparisonInfo> GetProcessedImages()
        {
            var processedImages = new List<ImageComparisonInfo>();

            foreach (var imageTaskWorker in _taskWorkers)
            {
                processedImages.AddRange(imageTaskWorker.GetProcessedImages());
            }

            return processedImages;
        }

        private void SendProcessedImagesToKeplerService(object sender, ElapsedEventArgs eventArgs)
        {
            var processedImages = GetProcessedImages().ToList();

            if (processedImages.Count > 0)
            {
                try
                {
                    var client = new RestClient(KeplerServiceUrl);
                    var request = new RestRequest("UpdateScreenShots", Method.POST);

                    var imageComparisonContract = new ImageComparisonContract() {ImageComparisonList = processedImages};

                    request.RequestFormat = DataFormat.Json;
                    request.AddJsonBody(imageComparisonContract);

                    client.Execute(request);
                }
                catch (Exception ex)
                {
                    EventLog.WriteEntry("Kepler.ImageProcessor.Service", ex.Message);
                }
            }
        }

        public void RemoveImagesFromProcessing(List<ImageComparisonInfo> imageComparisonList)
        {
            foreach (var imageTaskWorker in _taskWorkers)
            {
                imageTaskWorker.RemoveImagesFromProcessing(imageComparisonList);
            }
        }
    }
}