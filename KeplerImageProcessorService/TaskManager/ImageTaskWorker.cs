using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Kepler.Common.Models;
using KeplerImageProcessorService.ImgProcessor;

namespace KeplerImageProcessorService.TaskManager
{
    public class ImageTaskWorker
    {
        public int CountAvailableTask { get; }
        private ConcurrentBag<ImageInfo> imagesToProcess = new ConcurrentBag<ImageInfo>();
        private ConcurrentBag<ImageInfo> processedImages = new ConcurrentBag<ImageInfo>();

        public void AddImagesForProcessing(IEnumerable<ImageInfo> images)
        {
            images.ToList().ForEach(imagesToProcess.Add);
        }

        public void ProcessImages()
        {
            var imageProcessor = new ImageComparator();

            for (int index = 0; index < imagesToProcess.Count; index++)
            {
                ImageInfo currentImageToProcess;

                var attemptTakeImage = 0;
                do
                {
                    if (!imagesToProcess.TryTake(out currentImageToProcess))
                    {
                        attemptTakeImage++;
                    }
                    else
                    {
                        break;
                    }
                } while (attemptTakeImage < 3);

                imageProcessor.ImageInfo = currentImageToProcess;
                ImageInfo diffImage;

                try
                {
                    diffImage = imageProcessor.GetCompositeImageDiff();
                }
                catch (Exception ex)
                {
                    diffImage = new ImageInfo()
                    {
                        ErrorMessage = ex.Message,
                        ScreenShotId = currentImageToProcess.ScreenShotId
                    };
                }
            }
        }

        public IEnumerable<ImageInfo> GetProcessedImages()
        {
            lock (processedImages)
            {
                var returnImages = new List<ImageInfo>(processedImages.ToArray());
                processedImages = new ConcurrentBag<ImageInfo>();

                return returnImages;
            }
        }
    }
}