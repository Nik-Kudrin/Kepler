using System;
using System.Collections.Generic;
using Kepler.Core;

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

        public Build GetBuild(string id)
        {
            return buildRepo.Get(Convert.ToInt64(id));
        }

        public IEnumerable<Build> GetBuilds()
        {
            return buildRepo.FindAll();
        }

        public IEnumerable<Build> GetTestCase(string id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Build> GetTestCases(string testSuiteId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Build> GetTestSuite(string id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Build> GetTestSuites(string assemblyId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Build> GetTestAssembly(string assemblyId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Build> GetTestAssemblies(string buildId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Build> GetProject(string id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Build> GetProjects()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Build> GetBaseline(string id)
        {
            throw new NotImplementedException();
        }
    }
}