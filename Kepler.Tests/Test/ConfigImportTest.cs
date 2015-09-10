using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using FluentAssertions;
using Kepler.Core;
using Kepler.Models;
using Kepler.Service;
using Kepler.Service.Config;
using Newtonsoft.Json;
using NUnit.Framework;

namespace Kepler.Tests.Test
{
    public class ConfigImportTest : InitTest
    {
        [Test]
        public void TestSerialize()
        {
            var project = new Project()
            {
                Name = "SomeProjectName" + Guid.NewGuid().ToString(),
            };

            // We don't import builds
            // We don't import baseline

            // Test Assembly
            var assemblies = new List<TestAssembly>()
            {
                new TestAssembly()
                {
                    Name = "Some TestAssembly Name"
                }
            };


            // Test Suite
            var suites = new List<TestSuite>()
            {
                new TestSuite()
                {
                    Name = "Some test suite Name"
                }
            };

            // Test Cases
            var testCases = new List<TestCase>()
            {
                new TestCase()
                {
                    Name = "Test case name",
                }
            };


            // ScreenShots
            var screenShots = new List<ScreenShot>()
            {
                new ScreenShot()
                {
                    ImagePath = "Path To Image 1"
                },
                new ScreenShot()
                {
                    ImagePath = "Path to Image 2"
                }
            };

/*
            var projects = new TestImportConfig();
            projects.Projects.Add(project);


            Console.WriteLine("Project ===============");
            var json = JsonConvert.SerializeObject(projects, Formatting.Indented);
            Console.WriteLine(json);

            Console.WriteLine("Test Assembly ===============");
            json = JsonConvert.SerializeObject(assemblies, Formatting.Indented);
            Console.WriteLine(json);

            Console.WriteLine("Test Suites ===============");
            json = JsonConvert.SerializeObject(suites, Formatting.Indented);
            Console.WriteLine(json);

            Console.WriteLine("Test Cases ===============");
            json = JsonConvert.SerializeObject(testCases, Formatting.Indented);
            Console.WriteLine(json);

            Console.WriteLine("Screenshots ===============");
            json = JsonConvert.SerializeObject(screenShots, Formatting.Indented);
            Console.WriteLine(json);

            Console.WriteLine("Test Assembly ===============");
            json = JsonConvert.SerializeObject(assemblies, Formatting.Indented);
            Console.WriteLine(json);

            Console.WriteLine("Test Suites ===============");
            json = JsonConvert.SerializeObject(suites, Formatting.Indented);
            Console.WriteLine(json);

            Console.WriteLine("Test Cases ===============");
            json = JsonConvert.SerializeObject(testCases, Formatting.Indented);
            Console.WriteLine(json);

            Console.WriteLine("Screenshots ===============");
            json = JsonConvert.SerializeObject(screenShots, Formatting.Indented);
            Console.WriteLine(json);*/
        }

        [Test]
        public void ImportSimpleConfig()
        {
            var config = Path.Combine(BaseResourcePath, "simple_test_config.json");
            var deserializedObject = JsonConvert.DeserializeObject<TestImportConfig>(File.ReadAllText(config));

            var mapper = new ConfigMapper();
            var mappedProject = mapper.GetProjects(deserializedObject.Projects);
            // here we have to find project in DB and assign them ID


            var assemblies = deserializedObject.Projects.First().TestAssemblies;
            var mappedAssemblies = mapper.GetAssemblies(assemblies);
            // here we have to bind test assemblies with project id (parentObjId) based on Name of Project

            var suites = assemblies.First().TestSuites;
            var mappedSuites = mapper.GetSuites(suites);
            // here we have to bind suites with assemblies

            var cases = suites.First().TestCases;
            var mappedCases = mapper.GetCases(cases);

            var screenShots = cases.First().ScreenShots;

            deserializedObject.Should().NotBeNull();
        }
    }
}