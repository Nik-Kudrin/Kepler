using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Kepler.Common.Models;
using KeplerImageProcessorService.ImgProcessor;

namespace KeplerImageProcessorService.TaskManager
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
    }
}