﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Kepler.Common.Core;
using Kepler.Common.Error;
using Kepler.Common.Models;
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
        private ImageWorkerRepository workerRepository = ImageWorkerRepository.Instance;


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
            try
            {
                return buildRepo.FindAll();
            }
            catch (Exception ex)
            {
                EventLog.WriteEntry("Kepler.Service", $"Build error message: {ex.Message}. StackTrace: {ex.StackTrace}");
            }
            return null;
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

        public string CreateProject(string projectName)
        {
            if (projectRepository.Find(projectName).Any())
                return new ErrorMessage()
                {
                    Code = ErrorMessage.ErorCode.NotUniqueObjects,
                    ExceptionMessage = $"Project with name {projectName} already exist"
                }.ToString();

            try
            {
                var project = new Project() {Name = projectName};
                projectRepository.Insert(project);
            }
            catch (Exception ex)
            {
                return new ErrorMessage()
                {
                    Code = ErrorMessage.ErorCode.UndefinedError,
                    ExceptionMessage = $"Something bad happend. Exception: {ex.Message} {ex.StackTrace}"
                }.ToString();
            }

            return string.Empty;
        }

        #endregion

        public string ImportTestConfig(string testConfig)
        {
            var configImporter = new ConfigImporter();
            return configImporter.ImportConfig(testConfig);
        }

        public void RegisterImageWorker(string imageWorkerServiceUrl)
        {
            if (workerRepository.Find(imageWorkerServiceUrl).Count() == 0)
                workerRepository.Insert(new ImageWorker() {WorkerServiceUrl = imageWorkerServiceUrl});
        }

        #region Kepler Configs

        public string GetDiffImageSavingPath()
        {
            var diffImgPathToSaveProperty = KeplerSystemConfigRepository.Instance.Find("DiffImgPathToSave");
            return diffImgPathToSaveProperty == null ? "" : diffImgPathToSaveProperty.Value;
        }

        public void SetDiffImageSavingPath(string diffImageSavingPath)
        {
            var diffImgPathToSaveProperty = KeplerSystemConfigRepository.Instance.Find("DiffImgPathToSave");

            if (diffImgPathToSaveProperty == null)
                KeplerSystemConfigRepository.Instance.Insert(new KeplerSystemConfig("DiffImgPathToSave",
                    diffImageSavingPath));
            else
            {
                diffImgPathToSaveProperty.Value = diffImageSavingPath;
                KeplerSystemConfigRepository.Instance.Update(diffImgPathToSaveProperty);
                KeplerSystemConfigRepository.Instance.FlushChanges();
            }
        }

        #endregion
    }
}