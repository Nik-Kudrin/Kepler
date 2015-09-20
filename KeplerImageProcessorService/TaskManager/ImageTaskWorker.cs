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
        private ConcurrentBag<ImageInfo> _imagesToProcess = new ConcurrentBag<ImageInfo>();
        private ConcurrentBag<ImageInfo> _processedImages = new ConcurrentBag<ImageInfo>();

        public void AddImagesForProcessing(IEnumerable<ImageInfo> images)
        {
            images.ToList().ForEach(_imagesToProcess.Add);
        }

        public void ProcessImages()
        {
            var imageComparator = new ImageComparator();

            for (int index = 0; index < _imagesToProcess.Count; index++)
            {
                ImageInfo currentImageToProcess;

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

                imageComparator.ImageInfo = currentImageToProcess;
                _processedImages.Add(GetImageDiff(imageComparator));
            }
        }

        private ImageInfo GetImageDiff(ImageComparator imageComparator)
        {
            ImageInfo diffImage;

            try
            {
                diffImage = imageComparator.GetCompositeImageDiff();
            }
            catch (Exception ex)
            {
                diffImage = new ImageInfo()
                {
                    ErrorMessage = ex.Message,
                    ScreenShotId = imageComparator.ImageInfo.ScreenShotId
                };
            }

            return diffImage;
        }

        public IEnumerable<ImageInfo> GetProcessedImages()
        {
            lock (_processedImages)
            {
                var imagesToReturn = new List<ImageInfo>(_processedImages.ToArray());
                _processedImages = new ConcurrentBag<ImageInfo>();

                return imagesToReturn;
            }
        }
    }
}