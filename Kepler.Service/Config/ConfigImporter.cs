using System;
using System.Collections.Generic;
using System.Linq;
using Kepler.Common.Error;
using Kepler.Core;
using Kepler.Models;
using Kepler.Service.Config;
using Newtonsoft.Json;

namespace Kepler.Service
{
    public class ConfigImporter
    {
        /* private BuildRepository buildRepo = BuildRepository.Instance;
        private TestCaseRepository testCaseRepo = TestCaseRepository.Instance;
        private TestAssemblyRepository assemblyRepository = TestAssemblyRepository.Instance;
        private ProjectRepository projectRepository = ProjectRepository.Instance;
        private TestSuiteRepository testSuiteRepo = TestSuiteRepository.Instance;
        private ScreenShotRepository screenShotRepository = ScreenShotRepository.Instance;*/

        /// <summary>
        /// Import json config file
        /// </summary>
        /// <param name="jsonConfig"></param>
        /// <returns>Return empty string, if import was 'Ok', otherwise - error message</returns>
        public string ImportConfig(string jsonConfig)
        {
            TestImportConfig deserializedObject;
            try
            {
                deserializedObject = JsonConvert.DeserializeObject<TestImportConfig>(jsonConfig);
            }
            catch (Exception ex)
            {
                return new ErrorMessage() {Code = ErrorMessage.ErorCode.ParsingFileError, ExceptionMessage = ex.Message}.ToString();
            }


            var mapper = new ConfigMapper();
            var mappedProjects = mapper.GetProjects(deserializedObject.Projects).ToList();
            // here we have to find project in DB and assign them ID

            try
            {
                mappedProjects = BindImportedProjectWithExistedInDB(mappedProjects);
            }
            catch (Exception ex)
            {
                return ex.Message;
            }


            var assemblies = deserializedObject.Projects.First().TestAssemblies;
            var mappedAssemblies = mapper.GetAssemblies(assemblies);
            // here we have to bind test assemblies with project id (parentObjId) based on Name of Project

            var suites = assemblies.First().TestSuites;
            var mappedSuites = mapper.GetSuites(suites);
            // here we have to bind suites with assemblies

            var cases = suites.First().TestCases;
            var mappedCases = mapper.GetCases(cases);

            var screenShots = cases.First().ScreenShots;

            return "";
        }


        private List<Project> BindImportedProjectWithExistedInDB(List<Project> mappedProjects)
        {
            for (int index = 0; index < mappedProjects.Count(); index++)
            {
                var project = mappedProjects[index];

                var projectName = project.Name.Trim();

                if (projectName == "")
                {
                    throw new Exception(new ErrorMessage()
                    {
                        Code = ErrorMessage.ErorCode.ProjectDontHaveAName,
                        ExceptionMessage = ""
                    }.ToString());
                }

                project = ProjectRepository.Instance.Find(projectName).FirstOrDefault();
            }

            return mappedProjects;
        }


        // TODO: implement correct bindig between instances (parentObjId)

        // TODO: implement validation after config import and before store it in DB
    }
}