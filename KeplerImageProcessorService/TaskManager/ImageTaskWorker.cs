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
        public static int MaxImagesPerTask { get; }
        public int CountAvailableTask { get; }
        private ConcurrentBag<ImageInfo> imagesToProcess = new ConcurrentBag<ImageInfo>();
        private ConcurrentBag<ImageInfo> processedImages = new ConcurrentBag<ImageInfo>();

        static ImageTaskWorker()
        {
            MaxImagesPerTask = 30;
        }

        public ImageTaskWorker()
        {
            CountAvailableTask = MaxImagesPerTask;
        }

        public void AddImagesForProcessing(IEnumerable<ImageInfo> images)
        {
            images.ToList().ForEach(imagesToProcess.Add);
        }

        // TODO: Tasks should be run always. And go sleep, when list of image for processing is empty


        private void TaskProcessor()
        {
            while (imagesToProcess.Count > 0)
            {
                var task = Task<IEnumerable<ImageInfo>>.Factory.StartNew(ProcessImages);

                task.Start();
            }
        }


        private IEnumerable<ImageInfo> ProcessImages()
        {
            var imageProcessor = new ImageProcessor();

            for (int index = 0; index < MaxImagesPerTask; index++)
            {
                /* if (imagesToProcess.TryTake())

                imageProcessor.ImageInfo = imageInfo;
                yield return imageProcessor.GetCompositeImageDiff();*/
            }
        }
    }
}