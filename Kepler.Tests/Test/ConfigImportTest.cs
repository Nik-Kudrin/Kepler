using System;
using System.Web.Script.Serialization;
using Kepler.Models;
using Kepler.Tests.FixtureBuilder;
using Newtonsoft.Json;
using NUnit.Framework;

namespace Kepler.Tests.Test
{
    public class ConfigImportTest : InitTest
    {
        [Test]
        public void TestSerialize()
        {
            var project = new ProjectBuilder().BuildValid();


            var json = JsonConvert.SerializeObject(project, Formatting.Indented);
            Console.WriteLine(json);
        }
    }
}