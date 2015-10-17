﻿using System;
using System.IO;
using FluentAssertions;
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
            var request = new RestRequest("SetDiffImageSavingPath", Method.POST);
            request.RequestFormat = DataFormat.Json;
            request.AddBody("e:\\Temp\\ScreenCompareResult\\");

            var response = client.Execute(request);
            response.Content.Replace("\"", "").ShouldBeEquivalentTo("");

            // Create project
            request = new RestRequest("CreateProject", Method.POST);
            request.RequestFormat = DataFormat.Json;
            request.AddBody("Demo Project");

            response = client.Execute(request);
            response.Content.Replace("\"", "").ShouldBeEquivalentTo("");

            // Create branch
            request = new RestRequest("CreateBranch", Method.POST);
            request.RequestFormat = DataFormat.Json;
            request.AddQueryParameter("name", "Develop");
            request.AddQueryParameter("projectId", "1");

            response = client.Execute(request);
            response.Content.Replace("\"", "").ShouldBeEquivalentTo("");

            // Set branch as main
            request = new RestRequest("UpdateBranch", Method.POST);
            request.RequestFormat = DataFormat.Json;
            request.AddQueryParameter("name", "Develop");
            request.AddQueryParameter("newName", "Develop");
            request.AddQueryParameter("isMainBranch", "true");

            response = client.Execute(request);
            response.Content.Replace("\"", "").ShouldBeEquivalentTo("");

            // Register image worker
            request = new RestRequest("RegisterImageWorker", Method.POST);
            request.RequestFormat = DataFormat.Json;
            request.AddQueryParameter("name", "FirstWorker");
            request.AddQueryParameter("imageWorkerServiceUrl", "http://localhost:8900/Kepler.ImageProcessor.Service/");

            response = client.Execute(request);
            response.Content.Replace("\"", "").ShouldBeEquivalentTo("");
        }
    }
}