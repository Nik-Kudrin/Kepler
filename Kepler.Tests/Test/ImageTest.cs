using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using FluentAssertions;
using Kepler.Common.CommunicationContracts;
using Kepler.Common.Models;
using Kepler.ImageProcessor.Service.ImgProcessor;
using Kepler.ImageProcessor.Service.RestKeplerClient;
using Kepler.ImageProcessor.Service.TaskManager;
using Kepler.Service.Config;
using NUnit.Framework;
using RestSharp;

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
                    DiffImagePath = outputFile + "_" + index + ".png"
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
                DiffImagePath = outputFile + "_diff_ImageTaskWorkerTest.png",
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

            for (int index = 0; index < 3; index += 2)
            {
                var imageInfo = new ImageComparisonInfo()
                {
                    FirstImagePath = fileNames[index],
                    SecondImagePath = fileNames[index + 1],
                    DiffImagePath = outputFile + "_" + index + ".png"
                };

                images.Add(imageInfo);
            }
            var countImages = images.Count;

            var startDate = DateTime.Now;
            Console.WriteLine("Start Date: " + startDate);

            imageTaskGenerator.AddImagesForProcessing(images);

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


        [Test]
        public void GenerateJsonListOfImageComparison()
        {
            var message = new ImageComparisonContract()
            {
                ImageComparisonList = new List<ImageComparisonInfo>()
            };

            var fileNames = Directory.GetFiles(@"e:\Temp\Screen\");
            var images = new List<ImageComparisonInfo>();

            for (int index = 0; index < 10; index += 2)
            {
                var imageInfo = new ImageComparisonInfo()
                {
                    ScreenShotId = index,
                    FirstImagePath = fileNames[index],
                    SecondImagePath = fileNames[index + 1],
                    DiffImagePath = outputFile + "_" + index + ".png"
                };

                images.Add(imageInfo);
            }

            message.ImageComparisonList.AddRange(images);

            var client = new RestClient("http://localhost:8900/KeplerImageProcessorService/");
            var request = new RestRequest("AddImagesForDiffGeneration", Method.POST);

            request.RequestFormat = DataFormat.Json;
            request.AddJsonBody(message);

            client.Execute(request);
        }

        [Test]
        public void CorrectScreenShotSelectionTest()
        {
            var newScreenShots = new List<ScreenShot>();
            newScreenShots.AddRange(
                new[]
                {
                    new ScreenShot()
                    {
                        Name = "1",
                        BaseLineId = 1
                    },
                    new ScreenShot()
                    {
                        Name = "2",
                        BaseLineId = 3
                    },
                    new ScreenShot()
                    {
                        Name = "3",
                        BaseLineId = 3
                    },
                    new ScreenShot()
                    {
                        Name = "4",
                        BaseLineId = 5
                    },
                });

            var imagesComparisonContainer = new List<ImageComparisonInfo>();

            // group all screenshots by baseline
            var groupedNewScreenShots = newScreenShots.GroupBy(item => item.BaseLineId);

            foreach (var newBaselineScreenShot in groupedNewScreenShots)
            {
                Console.WriteLine(">>>>>> Group: " + newBaselineScreenShot.Key);

                var newScreenShotsForProcessing = newBaselineScreenShot.AsEnumerable().ToList();
                newScreenShotsForProcessing.ForEach(
                    item =>
                        Console.WriteLine(string.Format("screen: name={0}; baseline={1}", item.Name, item.BaseLineId)));
            }
        }
    }
}