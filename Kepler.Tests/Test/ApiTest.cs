using System;
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
    }
}