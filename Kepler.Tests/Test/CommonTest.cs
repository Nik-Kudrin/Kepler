using Kepler.Common.Models;
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

            var baseline = new BaseLine();
            BaseLineRepository.Instance.Insert(baseline);

            var branch = new Branch() {BaseLineId = baseline.Id, IsMainBranch = true, Name = "Master"};
            BranchRepository.Instance.Insert(branch);

            var project = new Project() {Name = "Some Project Name", MainBranchId = branch.Id};
            repo.Insert(project);

            baseline.BranchId = branch.Id;
            BaseLineRepository.Instance.UpdateAndFlashChanges(baseline);

            branch.ProjectId = project.Id;
            BranchRepository.Instance.UpdateAndFlashChanges(branch);
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