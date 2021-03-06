﻿using System;
using System.IO;
using FluentAssertions;
using Kepler.Common.CommunicationContracts;
using Kepler.Common.Util;
using Newtonsoft.Json;
using NUnit.Framework;
using RestSharp;

namespace Kepler.Tests.Test
{
    public class ApiTest : InitTest
    {
        [Test]
        public void CreateProject()
        {
            var client = new RestClient("http://localhost:8733/Kepler.Service/");
            var request = new RestRequest("CreateProject", Method.POST);

            request.RequestFormat = DataFormat.Json;
            request.AddBody("Some New Project _ " + Guid.NewGuid().ToString());

            var response = client.Execute(request);
            response.Content.Replace("\"", "").ShouldBeEquivalentTo("");
        }

        [Test]
        public void JsonSerializeExample()
        {
            var x = JsonConvert.SerializeObject(@"e:\Temp\ScreenCompareResult\");
            Console.WriteLine(x);
        }

        [Test]
        public void ImportTestConfig()
        {
            var buildConfigFile = File.ReadAllText(Path.Combine(BaseResourcePath, "test_config.json"));

            Console.WriteLine(JsonConvert.SerializeObject(buildConfigFile));

            var client = new RestClient("http://localhost:8733/Kepler.Service/");
            var request = new RestRequest("ImportTestConfig", Method.POST);

            request.RequestFormat = DataFormat.Json;
            request.AddBody(buildConfigFile);

            var response = client.Execute(request);
            response.Content.Replace("\"", "").ShouldBeEquivalentTo("");
        }


        [Test]
        public void InitDatabaseContent()
        {
            var client = new RestClient("http://localhost:8733/Kepler.Service/");

            // Set diff image path
            var request = new RestRequest("SetDiffImageSavingPath", Method.GET);
            request.RequestFormat = DataFormat.Json;
            request.AddQueryParameter("diffImageSavingPath", "\\\\NEON-PC\\diff");

            var response = client.Execute(request);
            response.Content.Replace("\"", "").ShouldBeEquivalentTo("");

            // Set source image path
            request = new RestRequest("SetSourceImageSavingPath", Method.GET);
            request.RequestFormat = DataFormat.Json;
            request.AddQueryParameter("sourceImageSavingPath", "\\\\NEON-PC\\origin");

            response = client.Execute(request);
            response.Content.Replace("\"", "").ShouldBeEquivalentTo("");

            // Set KeplerService url
            request = new RestRequest("SetKeplerServiceUrl", Method.GET);
            request.RequestFormat = DataFormat.Json;
            request.AddQueryParameter("url", "http://localhost:8733/Kepler.Service");

            response = client.Execute(request);
            response.Content.Replace("\"", "").ShouldBeEquivalentTo("");

            // Create project
            request = new RestRequest("CreateProject", Method.GET);
            request.RequestFormat = DataFormat.Json;
            request.AddQueryParameter("name", "Demo Project");

            response = client.Execute(request);
            response.Content.Replace("\"", "").ShouldBeEquivalentTo("");

            // Create branch
            request = new RestRequest("CreateBranch", Method.GET);
            request.RequestFormat = DataFormat.Json;
            request.AddQueryParameter("name", "Develop");
            request.AddQueryParameter("projectId", "1");

            response = client.Execute(request);
            response.Content.Replace("\"", "").ShouldBeEquivalentTo("");

            // Set branch as main
            request = new RestRequest("UpdateBranch", Method.GET);
            request.RequestFormat = DataFormat.Json;
            request.AddQueryParameter("id", "1");
            request.AddQueryParameter("newName", "Develop");
            request.AddQueryParameter("isMainBranch", "true");

            response = client.Execute(request);
            response.Content.Replace("\"", "").ShouldBeEquivalentTo("");

            // Register image worker
            request = new RestRequest("RegisterImageWorker", Method.GET);
            request.RequestFormat = DataFormat.Json;
            request.AddQueryParameter("name", "FirstWorker");
            request.AddQueryParameter("imageWorkerServiceUrl", "http://localhost:8900/Kepler.ImageProcessor.Service/");

            response = client.Execute(request);
            response.Content.Replace("\"", "").ShouldBeEquivalentTo("");
        }


        [Test]
        public void GetDiffImageSavingPath()
        {
            var client = new RestClient("http://localhost:8733/Kepler.Service");
            var request = new RestRequest("GetPreviewSavingPath", Method.GET);

            request.RequestFormat = DataFormat.Json;
            var response = client.Execute(request);

            Console.WriteLine(response.Content);
        }


        [Test]
        public void GetCleanScheduler()
        {
            var client = new RestClient("http://localhost:8733/Kepler.Service");
            var request = new RestRequest("GetCleanDataScheduler", Method.GET);
            request.RequestFormat = DataFormat.Json;
            request.AddQueryParameter("schedulerName", "logCleanScheduler");

            var response = client.Execute(request);
            var scheduler =
                new RestSharpDataContractJsonDeserializer().Deserialize<DataSchedulerContract>(response.Content);

            Console.WriteLine(
                $"Scheduler name {scheduler.Name}; Last start: {scheduler.LastStartTime}; Next start: {scheduler.NextStartTime}; Period: {scheduler.SchedulePeriod}");
        }
    }
}