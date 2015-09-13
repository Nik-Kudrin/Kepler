using Kepler.Core;
using Kepler.Core.Common;
using Kepler.Models;
using NUnit.Framework;

namespace Kepler.Tests.Test
{
    public class CommonTest : InitTest
    {
        [Test]
        public void CreateScreenShot()
        {
            var repo = ScreenShotRepository.Instance;

            var screenShot = new ScreenShot("ScreenShot_Inside_The_Case_Unique_Name", "some/path/to/screenshot");
            repo.Insert(screenShot);
        }

        [Test]
        public void CreateTestCase()
        {
            var repo = TestCaseRepository.Instance;

            repo.Insert(new TestCase("Test Case Name"));
            repo.Insert(new TestCase("Test Case Name 2"));
        }

        [Test]
        public void CreateTestSuite()
        {
            var repo = TestSuiteRepository.Instance;

            repo.Insert(new TestSuite("TestSuite_Name"));
        }

        [Test]
        public void CreateTestAssembly()
        {
            var repo = TestAssemblyRepository.Instance;

            repo.Insert(new TestAssembly("TestAssembly_Name"));
        }


        [Test]
        public void CreateProject()
        {
            var repo = ProjectRepository.Instance;

            var project = new Project() {Name = "Some Project Name"};
            repo.Insert(project);
        }

        [Test]
        public void CreateProjectBaseLine()
        {
            var repo = ProjectRepository.Instance;

            var project = new Project() {Name = "Project name"};
            project.BaseLine = new BaseLine();

            repo.Insert(project);
        }

        [Test]
        public void CreateBuild()
        {
            var repo = BuildRepository.Instance;

            var build = new Build() {Name = "Some build name"};

            repo.Insert(build);
        }

        [Test]
        public void CreateBuildWithSomeStatus()
        {
            var repo = BuildRepository.Instance;
            var build = new Build() {Name = "Build ..", Status = ObjectStatus.InProgress};

            repo.Insert(build);
        }


        [Ignore]
        [Test]
        public void CreateAllObjectsTree()
        {
        }
    }
}