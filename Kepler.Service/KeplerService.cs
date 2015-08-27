using System;
using System.Collections.Generic;
using Kepler.Core;
using Kepler.Models;

namespace Kepler.Service
{
    public class KeplerService : IKeplerService
    {
        private BuildRepository buildRepo = BuildRepository.Instance;
        private TestCaseRepository caseRepo = TestCaseRepository.Instance;
        private TestAssemblyRepository assemblyRepository = TestAssemblyRepository.Instance;
        private ProjectRepository projectRepository = ProjectRepository.Instance;
        private TestSuiteRepository suiteRepo = TestSuiteRepository.Instance;
        private ScreenShotRepository screenShotRepository = ScreenShotRepository.Instance;

        private long ConvertStringToLong(string number)
        {
            return Convert.ToInt64(number);
        }

        public Build GetBuild(string id)
        {
            return buildRepo.Get(ConvertStringToLong(id));
        }

        public IEnumerable<Build> GetBuilds()
        {
            return buildRepo.FindAll();
        }

        public TestCase GetTestCase(string id)
        {
            return caseRepo.Get(ConvertStringToLong(id));
        }

        public IEnumerable<TestCase> GetTestCases(string testSuiteId)
        {
            throw new NotImplementedException();
        }

        public TestSuite GetTestSuite(string id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<TestSuite> GetTestSuites(string assemblyId)
        {
            throw new NotImplementedException();
        }

        public TestAssembly GetTestAssembly(string assemblyId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<TestAssembly> GetTestAssemblies(string buildId)
        {
            throw new NotImplementedException();
        }

        public Project GetProject(string id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Project> GetProjects()
        {
            throw new NotImplementedException();
        }

        public BaseLine GetBaseline(string id)
        {
            throw new NotImplementedException();
        }
    }
}