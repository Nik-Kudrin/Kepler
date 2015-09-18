using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;
using Kepler.Common.Models;
using KeplerImageProcessorService.ImgProcessor;

namespace KeplerImageProcessorService.TaskManager
{
    public class ImageTaskGenerator
    {
        public static int MaxImagesPerTask { get; }
        public int CountAvailableTask { get; }
        private ConcurrentBag<ImageInfo> imagesToProcess;
        private ConcurrentBag<ImageInfo> processedImages;

        static ImageTaskGenerator()
        {
            MaxImagesPerTask = 30;
        }

        public ImageTaskGenerator(IEnumerable<ImageInfo> images)
        {
            CountAvailableTask = MaxImagesPerTask;

            this.imagesToProcess = new ConcurrentBag<ImageInfo>(images);
        }

       /* private void TaskGenerator()
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
                yield return imageProcessor.GetCompositeImageDiff();#1#
            }
        }*/
    }
}