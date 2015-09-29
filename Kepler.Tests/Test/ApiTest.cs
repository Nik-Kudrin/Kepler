using System;
using FluentAssertions;
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
    }
}