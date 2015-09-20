using System.Collections.Generic;
using System.IO;
using System.Linq;
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
                var imageInfo = new ImageInfo()
                {
                    FirstImagePath = fileNames[index],
                    SecondImagePath = fileNames[index + 1],
                    DiffImgPathToSave = outputFile + "_" + index + ".png"
                };

                imageProcessor.ImageInfo = imageInfo;

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

            var imagesForProcessing = new List<ImageInfo>();
            imagesForProcessing.Add(new ImageInfo()
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
        }
    }
}