using System;
using System.Collections.Generic;
using System.Linq;
using System.Timers;
using Kepler.Common.Core;
using Kepler.Common.Models;
using Kepler.Core;
using Kepler.Core.Common;
using RestSharp;

namespace Kepler.Service.Core
{
    public class BuildExecutor
    {
        private static BuildExecutor _executor;
        private static Timer _timer;

        private static BuildExecutor GetExecutor()
        {
            _executor = _executor ?? new BuildExecutor();
            return _executor;
        }

        private BuildExecutor()
        {
            _timer = new Timer();
            _timer.Interval = 10000; //every 10 sec
            _timer.Elapsed += SendComparisonInfoToWorkers;
            _timer.Enabled = true;
        }


        private void SendComparisonInfoToWorkers(object sender, ElapsedEventArgs eventArgs)
        {
            var workers = ImageWorkerRepository.Instance.FindAll()
                .Where(worker => worker.Status == ImageWorker.WorkerStatus.Available);

            var screenShots = GetAllInQueueScreenShots();
            var imageComparisonContainers = ConvertScreenShotsToImageComparison(screenShots);

            // TODO: split all screenshots for comparison uniformly for all workers

            foreach (var imageWorker in workers)
            {
                var client = new RestClient("http://localhost:8900/KeplerImageProcessorService/");
                var request = new RestRequest("AddImagesForDiffGeneration", Method.POST);

                request.RequestFormat = DataFormat.Json;
                request.AddJsonBody(message);

                client.Execute(request);
            }
        }


        public IEnumerable<ImageComparisonInfo> ConvertScreenShotsToImageComparison(IEnumerable<ScreenShot> screenShots)
        {
            // TODO
        }

        public IEnumerable<ScreenShot> GetAllInQueueScreenShots()
        {
            // TODO:  create list of image comparison objects (based on In queue builds and screenshots for them)
        }

        private IEnumerable<Build> GetInQueueBuilds()
        {
            return BuildRepository.Instance.Find(build => build.Status == ObjectStatus.InQueue);
        }

        private IEnumerable<ScreenShot> GetInQueueScreenShotsForBuild(long buildId)
        {
            return ScreenShotRepository.Instance.Find(screenShot => screenShot.BuildId == buildId &&
                                                                    screenShot.Status == ObjectStatus.InQueue);
        }
    }
}