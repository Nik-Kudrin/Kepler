using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Timers;
using Kepler.Common.CommunicationContracts;
using Kepler.Common.Models;
using Kepler.Common.Models.Common;
using Kepler.Common.Repository;
using Kepler.Service.RestWorkerClient;
using RestSharp;
using Timer = System.Timers.Timer;

namespace Kepler.Service.Core
{
    public class BuildExecutor
    {
        private static BuildExecutor _executor;
        private static Timer _sendScreenShotsForProcessingTimer;
        private static Timer _updateObjectStatusesTimer;
        public static string DiffImageSavingPath { get; set; }
        public static string PreviewImageSavingPath { get; set; }

        static BuildExecutor()
        {
            GetExecutor();
        }


        public static BuildExecutor GetExecutor()
        {
            _executor = _executor ?? new BuildExecutor();
            return _executor;
        }

        private BuildExecutor()
        {
            _sendScreenShotsForProcessingTimer = new Timer();
            _sendScreenShotsForProcessingTimer.Interval = 5000; //every 10 sec
            _sendScreenShotsForProcessingTimer.Elapsed += SendComparisonInfoToWorkers;
            _sendScreenShotsForProcessingTimer.Enabled = true;

            _updateObjectStatusesTimer = new Timer();
            _updateObjectStatusesTimer.Interval = 15000; //every 15 sec
            _updateObjectStatusesTimer.Elapsed += UpdateObjectsStatuses;
            _updateObjectStatusesTimer.Enabled = true;

            UpdateKeplerServiceUrlOnWorkers();
            UpdateDiffImagePath();
        }


        [MethodImpl(MethodImplOptions.Synchronized)]
        private void UpdateObjectsStatuses(object sender, ElapsedEventArgs eventArgs)
        {
            ObjectStatusUpdater.UpdateAllObjectStatusesToActual();
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        private void SendComparisonInfoToWorkers(object sender, ElapsedEventArgs eventArgs)
        {
            var workers = ImageWorkerRepository.Instance.FindAll()
                .Where(worker => worker.WorkerStatus == ImageWorker.StatusOfWorker.Available).ToList();

            var screenShots = ScreenShotRepository.Instance.GetAllInQueueScreenShots();
            var imageComparisonContainers = ConvertScreenShotsToImageComparison(screenShots).ToList();

            if (workers.Count == 0)
                return;

            // split all screenshots for comparison uniformly for all workers
            var imgComparisonPerWorker = imageComparisonContainers.Count()/workers.Count();
            if (imgComparisonPerWorker == 0)
                imgComparisonPerWorker = imageComparisonContainers.Count();


            int workerIndex = 0;
            while (imageComparisonContainers.Any())
            {
                var jsonMessage = new ImageComparisonContract()
                {
                    ImageComparisonList = imageComparisonContainers.Take(imgComparisonPerWorker).ToList()
                };

                var requestIsSuccessfull = true;
                try
                {
                    var client = new RestClient(workers[workerIndex++].WorkerServiceUrl);
                    var request = new RestRequest("AddImagesForDiffGeneration", Method.POST);

                    request.RequestFormat = DataFormat.Json;
                    request.AddJsonBody(jsonMessage);

                    client.Execute(request);
                }
                catch (Exception ex)
                {
                    requestIsSuccessfull = false;
                }

                if (requestIsSuccessfull)
                {
                    if (imageComparisonContainers.Count() < imgComparisonPerWorker)
                        imageComparisonContainers.Clear();
                    else
                    {
                        imageComparisonContainers.RemoveRange(0, imgComparisonPerWorker);
                    }
                }

                if (workerIndex == workers.Count)
                    workerIndex = 0;
            }
        }


        /// <summary>
        /// Iterate through all newly imported screenshots and create for them image comparison info object (for future diff processing)
        /// </summary>
        /// <param name="newScreenShots"></param>
        /// <returns>List of image comparison info, that will be used in diff processing service</returns>
        public IEnumerable<ImageComparisonInfo> ConvertScreenShotsToImageComparison(
            IEnumerable<ScreenShot> newScreenShots)
        {
            var imagesComparisonContainer = new List<ImageComparisonInfo>();

            // group all screenshots by baseline
            var groupedNewScreenShots = newScreenShots.GroupBy(item => item.BaseLineId);

            // Iterate through each group of baseline
            foreach (var newBaselineScreenShot in groupedNewScreenShots)
            {
                var oldPassedBaselineScreenShots =
                    ScreenShotRepository.Instance.GetBaselineScreenShots(newBaselineScreenShot.Key);

                var newScreenShotsForProcessing = newBaselineScreenShot.AsEnumerable().ToList();

                // if baseline is "empty" - just set screenshots as "passed"
                /* if (oldPassedBaselineScreenShots == null || !oldPassedBaselineScreenShots.Any())
                {
                    newScreenShotsForProcessing.ForEach(item =>
                    {
                        item.Status = ObjectStatus.Passed;
                        item.BaseLineImagePath = item.ImagePath;
                        item.IsLastPassed = true;
                    });
                    ScreenShotRepository.Instance.Update(newScreenShotsForProcessing);
                }
                else
                {*/
                newScreenShotsForProcessing.ForEach(item => item.Status = ObjectStatus.InProgress);
                ScreenShotRepository.Instance.Update(newScreenShotsForProcessing);

                imagesComparisonContainer.AddRange(GenerateImageComparison(newScreenShotsForProcessing,
                    oldPassedBaselineScreenShots.ToList()));
//                }

                ScreenShotRepository.Instance.FlushChanges();
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
            var resultImagesForComparison = new List<ImageComparisonInfo>();

            foreach (var newScreenShot in newScreenShotsForProcessing)
            {
                var oldScreenShot = oldPassedBaselineScreenShots.Find(oldScreen => oldScreen.Name == newScreenShot.Name);

                ImageComparisonInfo imageComparison = null;

                if (oldScreenShot == null)
                {
                    newScreenShot.Status = ObjectStatus.Passed;
                    newScreenShot.BaseLineImagePath = newScreenShot.ImagePath;
                    newScreenShot.IsLastPassed = true;

                    imageComparison = new ImageComparisonInfo()
                    {
                        ScreenShotName = newScreenShot.OriginalName,
                        LastPassedScreenShotId = newScreenShot.Id,
                        FirstImagePath = newScreenShot.ImagePath,
                        FirstPreviewPath = newScreenShot.PreviewImagePath,
                        SecondImagePath = newScreenShot.ImagePath,
                        SecondPreviewPath = newScreenShot.PreviewImagePath,
                        DiffImagePath = DiffImageSavingPath,
                        DiffPreviewPath = PreviewImageSavingPath,
                        ScreenShotId = newScreenShot.Id,
                    };
                }
                else
                {
                    if (string.IsNullOrEmpty(oldScreenShot.PreviewImagePath))
                        oldScreenShot.PreviewImagePath = PreviewImageSavingPath;

                    imageComparison = new ImageComparisonInfo()
                    {
                        ScreenShotName = newScreenShot.OriginalName,
                        LastPassedScreenShotId = oldScreenShot.Id,
                        FirstImagePath = oldScreenShot.ImagePath,
                        FirstPreviewPath = oldScreenShot.PreviewImagePath,
                        SecondImagePath = newScreenShot.ImagePath,
                        SecondPreviewPath = PreviewImageSavingPath,
                        DiffImagePath = DiffImageSavingPath,
                        DiffPreviewPath = PreviewImageSavingPath,
                        ScreenShotId = newScreenShot.Id,
                    };

                    newScreenShot.BaseLineImagePath = oldScreenShot.ImagePath;
                }
                resultImagesForComparison.Add(imageComparison);
                ScreenShotRepository.Instance.Update(newScreenShot);
            }

            ScreenShotRepository.Instance.FlushChanges();

            return resultImagesForComparison;
        }

        public void UpdateKeplerServiceUrlOnWorkers()
        {
            var workers = ImageWorkerRepository.Instance.FindAll()
                .Where(worker => worker.WorkerStatus == ImageWorker.StatusOfWorker.Available).ToList();

            foreach (var imageWorker in workers)
            {
                var restImageProcessorClient = new RestImageProcessorClient(imageWorker.WorkerServiceUrl);
                restImageProcessorClient.SetKeplerServiceUrl();
            }
        }

        public void UpdateDiffImagePath()
        {
            var keplerService = new KeplerService();

            DiffImageSavingPath = keplerService.GetDiffImageSavingPath();
            PreviewImageSavingPath = keplerService.GetPreviewSavingPath();

            if (string.IsNullOrEmpty(DiffImageSavingPath))
                return;

            if (!Directory.Exists(DiffImageSavingPath))
            {
                Directory.CreateDirectory(DiffImageSavingPath);
                Directory.CreateDirectory(PreviewImageSavingPath);
            }
        }
    }
}