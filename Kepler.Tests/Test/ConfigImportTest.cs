using System;
using System.Collections.Generic;
using System.IO;
using Kepler.Common.Models;
using Kepler.Service;
using Kepler.Service.Config;
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
                Name = "Some Project Name" + Guid.NewGuid().ToString(),
            };

            var assemblies = new List<TestAssembly>()
            {
                new TestAssembly()
                {
                    Name = "Some TestAssembly Name"
                }
            };

            var suites = new List<TestSuite>()
            {
                new TestSuite()
                {
                    Name = "Some test suite Name"
                }
            };

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
        }

        [Test]
        public void ImportSimpleConfig()
        {
//            new CommonTest().CreateProject();
            var config = File.ReadAllText(Path.Combine(BaseResourcePath, "simple_test_config.json"));

            var configImporter = new ConfigImporter();
            var errorMessage = configImporter.ImportConfig(config);
            Console.WriteLine(errorMessage);
        }
    }
}