using System;
using System.Linq;
using Kepler.Models;
using NUnit.Framework;

namespace Kepler.Tests.Core
{
    internal class ProjectTests : InitTest
    {
        [Test]
        public void CreateProject()
        {
/*            var screenShot = new ScreenShot("ScreenShot_Inside_The_Case_Unique_Name", "some/path/to/screenshot");

            var testCase = new TestCase("TestCase_Name");
            testCase.ScreenShots.Add(1, screenShot);

            var testSuite = new TestSuite("TestSuite_Name");
            testSuite.TestCases.Add(0, testCase);

            var testAssembly = new TestAssembly("TestAssembly_Name");*/
        }


        [Test]
        public void ReadFromDbTest()
        {
            using (var db = new KeplerDataContext())
            {
                var blog = new Project() { Name = "Some project"};
                db.Projects.Add(blog);
                db.SaveChanges();


                var query = from b in db.Projects
                    orderby b.Name
                    select b;

                Console.WriteLine("All blogs in the database:");
                foreach (var item in query)
                {
                    Console.WriteLine(item.Name);
                }
            }
        }


        private void Serialization()
        {
            /*var path = Path.Combine(BasePath, @"Resources\TestData.xml");

            DataContractSerializer xs = new DataContractSerializer(typeof (TestCase));

            var settings = new XmlWriterSettings()
            {
                Indent = true,
                Encoding = Encoding.UTF8,
                CloseOutput = true,
                NewLineHandling = NewLineHandling.Entitize
            };

            var writer = XmlWriter.Create(path, settings);

            xs.WriteObject(writer, testCase);
            writer.Flush();
            writer.Close();*/


            /*            var serializedConfig = EntityXmlSerializer.Serialize(testCase);
                        File.WriteAllText(Path.Combine(BasePath, @"Resources\TestData.xml"), serializedConfig);*/
        }
    }
}