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
        private static string DiffImageSavingPath;

        static BuildExecutor()
        {
            GetExecutor();
            DiffImageSavingPath = new KeplerService().GetDiffImageSavingPath();
        }

        public static BuildExecutor GetExecutor()
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


        public IEnumerable<ImageComparisonInfo> ConvertScreenShotsToImageComparison(IEnumerable<ScreenShot> newScreenShots)
        {
            // select all screenshot for current baseline ID in status = Success
            // find the screenshot with the same name and the same baseline

            // if no one screenshot for baseline in status = Success, then assign current screenshot status = Success
            //(and do not send them for image processing)


            var imagesComparisonContainer = new List<ImageComparisonInfo>();

            // group all screenshots by baseline
            var groupedNewScreenShots = newScreenShots.GroupBy(item => item.BaseLineId);

            foreach (var newBaselineScreenShot in groupedNewScreenShots)
            {
                var oldPassedBaselineScreenShots = ScreenShotRepository.Instance
                    .Find(item => item.BaseLineId == newBaselineScreenShot.Key &&
                                  item.Status == ObjectStatus.Passed);

                var newScreenShotsForProcessing = newBaselineScreenShot.AsEnumerable().ToList();

                if (oldPassedBaselineScreenShots == null || !oldPassedBaselineScreenShots.Any())
                {
                    newScreenShotsForProcessing.ForEach(item => item.Status = ObjectStatus.Passed);
                    newScreenShotsForProcessing.ForEach(ScreenShotRepository.Instance.Update);

                    ScreenShotRepository.Instance.FlushChanges();
                }
                else
                {
                    imagesComparisonContainer.AddRange(GenerateImageComparison(newScreenShotsForProcessing, oldPassedBaselineScreenShots.ToList()));
                }
            }

            return imagesComparisonContainer;
        }

        /// <summary>
        /// Analyze two list of screenshots from the same baseline: new (In queue) and old (Passed)
        /// </summary>
        /// <param name="newScreenShotsForProcessing"></param>
        /// <param name="oldPassedBaselineScreenShots"></param>
        /// <returns>List of image comparison for diff generating</returns>
        private IEnumerable<ImageComparisonInfo> GenerateImageComparison(List<ScreenShot> newScreenShotsForProcessing,
            List<ScreenShot> oldPassedBaselineScreenShots)
        {
            var imagesForComparison = new List<ImageComparisonInfo>();

            foreach (var newScreenShot in newScreenShotsForProcessing)
            {
                var oldScreenShot = oldPassedBaselineScreenShots.Find(oldScreen => oldScreen.Name == newScreenShot.Name);
                if (oldScreenShot == null)
                {
                    newScreenShot.Status = ObjectStatus.Passed;
                    ScreenShotRepository.Instance.Update(newScreenShot);
                }
                else
                {
                    var imageComparison = new ImageComparisonInfo()
                    {
                        FirstImagePath = newScreenShot.ImagePath,
                        SecondImagePath = oldScreenShot.ImagePath,
                        DiffImgPathToSave = DiffImageSavingPath,
                        ScreenShotId = newScreenShot.Id,
                    };

                    imagesForComparison.Add(imageComparison);
                }
            }

            ScreenShotRepository.Instance.FlushChanges();

            return imagesForComparison;
        }


        public IEnumerable<Build> GetInQueueBuilds()
        {
            return BuildRepository.Instance.Find(build => build.Status == ObjectStatus.InQueue);
        }

        public IEnumerable<ScreenShot> GetInQueueScreenShotsForBuild(long buildId)
        {
            return ScreenShotRepository.Instance.Find(screenShot => screenShot.BuildId == buildId &&
                                                                    screenShot.Status == ObjectStatus.InQueue);
        }

        public IEnumerable<ScreenShot> GetAllInQueueScreenShots()
        {
            return ScreenShotRepository.Instance.Find(screenShot => screenShot.Status == ObjectStatus.InQueue);
        }
    }
}