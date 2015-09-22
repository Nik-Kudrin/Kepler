using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using FluentAssertions;
using Kepler.Common.Models;
using KeplerImageProcessorService.ImgProcessor;
using KeplerImageProcessorService.TaskManager;
using NUnit.Framework;

namespace Kepler.Tests.Test
{
    public class ImageTest : InitTest
    {
        private string outputFile = @"e:\Temp\ScreenCompareResult\result";

        [Test]
        public void DiffImageTest()
        {
            var imageProcessor = new ImageComparator();

            var fileNames = Directory.GetFiles(@"e:\Temp\Screen\");

            for (int index = 0; index < fileNames.Length - 3;)
            {
                var imageInfo = new ImageComparisonInfo()
                {
                    FirstImagePath = fileNames[index],
                    SecondImagePath = fileNames[index + 1],
                    DiffImgPathToSave = outputFile + "_" + index + ".png"
                };

                imageProcessor.ImageComparisonInfo = imageInfo;

                var isImageDifferent = imageProcessor.GetCompositeImageDiff();
                index += 2;
            }
        }


        [Test]
        public void ImageTaskWorkerTest()
        {
            var imageWorker = new ImageTaskWorker();

            imageWorker.ProcessImages();
            imageWorker.GetProcessedImages().Should().BeEmpty();

            var imagesForProcessing = new List<ImageComparisonInfo>();
            imagesForProcessing.Add(new ImageComparisonInfo()
            {
                DiffImgPathToSave = outputFile + "_diff_ImageTaskWorkerTest.png",
                FirstImagePath = @"e:\Temp\ScreenShot_Samples\ElementFinder_2015-07-29_15-51-00.png",
                SecondImagePath = @"e:\Temp\ScreenShot_Samples\ElementFinder_2015-07-29_15-51-21.png"
            });

            imageWorker.AddImagesForProcessing(imagesForProcessing);
            imageWorker.GetProcessedImages().Should().BeEmpty();

            imageWorker.ProcessImages();
            var image = imageWorker.GetProcessedImages().First();
            image.ErrorMessage.Should().BeNullOrEmpty();
            image.IsImagesDifferent.Should().BeTrue();
        }

        [Test]
        public void TaskGeneratorImage()
        {
            var imageTaskGenerator = TaskGenerator.GetTaskGenerator;

            var fileNames = Directory.GetFiles(@"e:\Temp\Screen\");
            var images = new List<ImageComparisonInfo>();

            for (int index = 0; index < 7; index += 2)
            {
                var imageInfo = new ImageComparisonInfo()
                {
                    FirstImagePath = fileNames[index],
                    SecondImagePath = fileNames[index + 1],
                    DiffImgPathToSave = outputFile + "_" + index + ".png"
                };

                images.Add(imageInfo);
            }
            var countImages = images.Count;

            var startDate = DateTime.Now;
            Console.WriteLine("Start Date: " + startDate);

            imageTaskGenerator.AddImagesToProcess(images);

            Console.WriteLine("Max images count " + countImages);

            var processedImagesCount = 0;
            do
            {
                Thread.Sleep(5000);

                processedImagesCount += imageTaskGenerator.GetProcessedImages().Count();
                Console.WriteLine("Processed images: " + processedImagesCount);
            } while (processedImagesCount < countImages);

            var finishDate = DateTime.Now;
            Console.WriteLine("Finish Date: " + finishDate);
            Console.WriteLine("Test duration: " + (finishDate - startDate));
        }
    }
}