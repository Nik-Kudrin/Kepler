using Kepler.Core;
using Kepler.Models;
using NUnit.Framework;

namespace Kepler.Tests.Core
{
    public class CommonTest : InitTest
    {
        [Test]
        public void CreateScreenShot()
        {
            using (var db = new KeplerDataContext())
            {
                var screenShot = new ScreenShot("ScreenShot_Inside_The_Case_Unique_Name", "some/path/to/screenshot");
                db.ScreenShots.Add(screenShot);
                db.SaveChanges();
            }
        }

        [Test]
        public void CreateTestCase()
        {
            using (var db = new KeplerDataContext())
            {
                db.TestCases.Add(new TestCase("Test Case Name"));
                db.TestCases.Add(new TestCase("Test Case Name 2"));
                db.SaveChanges();
            }
        }

        [Test]
        public void CreateTestSuite()
        {
            using (var db = new KeplerDataContext())
            {
                db.TestSuites.Add(new TestSuite("TestSuite_Name"));
                db.SaveChanges();
            }
        }

        [Test]
        public void CreateTestAssembly()
        {
            using (var db = new KeplerDataContext())
            {
                db.TestAssemblies.Add(new TestAssembly("TestAssembly_Name"));
                db.SaveChanges();
            }
        }


        [Test]
        public void CreateProject()
        {
            using (var db = new KeplerDataContext())
            {
                var project = new Project() {Name = "Some project"};
                db.Projects.Add(project);
                db.SaveChanges();

                /*var query = from b in db.Projects
                    orderby b.Name
                    select b;

                Console.WriteLine("All blogs in the database:");
                foreach (var item in query)
                {
                    Console.WriteLine(item.Name);
                }*/
            }
        }

        [Test]
        public void CreateProjectBaseLine()
        {
            using (var db = new KeplerDataContext())
            {
                var project = new Project() {Name = "Project name"};
                project.BaseLine = new BaseLine();

                db.Projects.Add(project);
                db.SaveChanges();
            }
        }

        [Test]
        public void CreateBuild()
        {
            using (var db = new KeplerDataContext())
            {
                var screenShot = new ScreenShot("ScreenShot_Inside_The_Case_Unique_Name", "some/path/to/screenshot");
            }
        }


        [Test]
        public void CreateAllObjectsTree()
        {
            using (var db = new KeplerDataContext())
            {
                var screenShot = new ScreenShot("ScreenShot_Inside_The_Case_Unique_Name", "some/path/to/screenshot");
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