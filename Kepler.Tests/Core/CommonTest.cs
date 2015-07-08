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
            var repo = ScreenShotRepository.Instance;

            var screenShot = new ScreenShot("ScreenShot_Inside_The_Case_Unique_Name", "some/path/to/screenshot");
            repo.Add(screenShot);
            repo.FlushChanges();
        }

        [Test]
        public void CreateTestCase()
        {
            var repo = TestCaseRepository.Instance;

            repo.Add(new TestCase("Test Case Name"));
            repo.Add(new TestCase("Test Case Name 2"));
            repo.FlushChanges();
        }

        [Test]
        public void CreateTestSuite()
        {
            var repo = TestSuiteRepository.Instance;

            repo.Add(new TestSuite("TestSuite_Name"));
            repo.FlushChanges();
        }

        [Test]
        public void CreateTestAssembly()
        {
            var repo = TestAssemblyRepository.Instance;

            repo.Add(new TestAssembly("TestAssembly_Name"));
            repo.FlushChanges();
        }


        [Test]
        public void CreateProject()
        {
            var repo = ProjectRepository.Instance;

            var project = new Project() {Name = "Some project"};
            repo.Add(project);
            repo.FlushChanges();
        }

        [Test]
        public void CreateProjectBaseLine()
        {
            var repo = ProjectRepository.Instance;

            var project = new Project() {Name = "Project name"};
            project.BaseLine = new BaseLine();

            repo.Add(project);
            repo.FlushChanges();
        }

        [Test]
        public void CreateBuild()
        {
            var repo = BuildRepository.Instance;

            var build = new Build() {Name = "Some build name"};

            repo.Add(build);
            repo.FlushChanges();
        }


        [Ignore]
        [Test]
        public void CreateAllObjectsTree()
        {
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