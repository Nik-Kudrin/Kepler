using System;
using System.Collections.Generic;
using Kepler.Core;
using Kepler.Models;

namespace Kepler.Service
{
    public class KeplerService : IKeplerService
    {
        private BuildRepository buildRepo = BuildRepository.Instance;
        private TestCaseRepository testCaseRepo = TestCaseRepository.Instance;
        private TestAssemblyRepository assemblyRepository = TestAssemblyRepository.Instance;
        private ProjectRepository projectRepository = ProjectRepository.Instance;
        private TestSuiteRepository testSuiteRepo = TestSuiteRepository.Instance;
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

        #region TestCase

        public TestCase GetTestCase(string id)
        {
            return testCaseRepo.Get(ConvertStringToLong(id));
        }

        public IEnumerable<TestCase> GetTestCases(string testSuiteId)
        {
            return testCaseRepo.Find(ConvertStringToLong(testSuiteId));
        }

        #endregion

        #region TestSuite

        public TestSuite GetTestSuite(string id)
        {
            return testSuiteRepo.Get(ConvertStringToLong(id));
        }

        public IEnumerable<TestSuite> GetTestSuites(string assemblyId)
        {
            return testSuiteRepo.Find(ConvertStringToLong(assemblyId));
        }

        #endregion

        #region TestAssembly

        public TestAssembly GetTestAssembly(string assemblyId)
        {
            return assemblyRepository.Get(ConvertStringToLong(assemblyId));
        }


        public IEnumerable<TestAssembly> GetTestAssemblies(string buildId)
        {
            return assemblyRepository.Find(ConvertStringToLong(buildId));
        }

        #endregion

        #region Project

        public Project GetProject(string id)
        {
            return projectRepository.Get(ConvertStringToLong(id));
        }

        public IEnumerable<Project> GetProjects()
        {
            return projectRepository.FindAll();
        }

        #endregion
    }
}