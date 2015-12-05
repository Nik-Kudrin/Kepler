using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Kepler.Common.CommunicationContracts;
using Kepler.ImageProcessor.Service.ImgProcessor;
using Kepler.ImageProcessor.Service.RestKeplerClient;

namespace Kepler.ImageProcessor.Service.TaskManager
{
    public class ImageTaskWorker
    {
        public Task AssignedTask { get; set; }
        private ConcurrentBag<ImageComparisonInfo> _imagesToProcess = new ConcurrentBag<ImageComparisonInfo>();
        private List<ImageComparisonInfo> _processedImages = new List<ImageComparisonInfo>();

        public void AddImagesForProcessing(IEnumerable<ImageComparisonInfo> images)
        {
            images.ToList().ForEach(_imagesToProcess.Add);
        }

        public void ProcessImages()
        {
            var imageComparator = new ImageComparator();

            for (int index = 0; index < _imagesToProcess.Count; index++)
            {
                ImageComparisonInfo currentImageToProcess;

                var attemptTakeImage = 0;
                do
                {
                    if (!_imagesToProcess.TryTake(out currentImageToProcess))
                    {
                        attemptTakeImage++;
                    }
                    else
                    {
                        break;
                    }
                } while (attemptTakeImage < 3);

                imageComparator.ImageComparisonInfo = currentImageToProcess;

                var imageDiff = GetImageDiff(imageComparator);
                lock (_processedImages)
                {
                    _processedImages.Add(imageDiff);
                }
            }
        }

        private ImageComparisonInfo GetImageDiff(ImageComparator imageComparator)
        {
            ImageComparisonInfo diffImage;

            try
            {
                diffImage = imageComparator.GetCompositeImageDiff();
            }
            catch (Exception ex)
            {
                diffImage = new ImageComparisonInfo()
                {
                    ErrorMessage = ex.Message,
                    ScreenShotId = imageComparator.ImageComparisonInfo.ScreenShotId
                };

                new RestKeplerServiceClient().LogError($"{ex.Message} {ex.StackTrace}");
            }

            return diffImage;
        }

        public IEnumerable<ImageComparisonInfo> GetProcessedImages()
        {
            lock (_processedImages)
            {
                var imagesToReturn = new List<ImageComparisonInfo>(_processedImages.ToArray());
                _processedImages.Clear();

                return imagesToReturn;
            }
        }

        public void RemoveImagesFromProcessing(List<ImageComparisonInfo> imageComparisonList)
        {
            var idsScreenShotsToRemove = imageComparisonList.Select(item => item.ScreenShotId);

            var updatedImagesCollection = _imagesToProcess.ToList();
            updatedImagesCollection.RemoveAll(item => idsScreenShotsToRemove.Contains(item.ScreenShotId));

            _imagesToProcess = new ConcurrentBag<ImageComparisonInfo>(updatedImagesCollection);
        }
    }
}