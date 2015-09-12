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
            TestImportConfig importedConfig;
            try
            {
                importedConfig = JsonConvert.DeserializeObject<TestImportConfig>(jsonConfig);
            }
            catch (Exception ex)
            {
                return new ErrorMessage() {Code = ErrorMessage.ErorCode.ParsingFileError, ExceptionMessage = ex.Message}.ToString();
            }

            var validationErrorMessage = ValidateImportedConfigObjects(importedConfig);
            if (validationErrorMessage != "")
                return validationErrorMessage;

            var mapper = new ConfigMapper();
            var mappedProjects = mapper.GetProjects(importedConfig.Projects).ToList();

            try
            {
                mappedProjects = BindImportedProjectWithExistedInDB(mappedProjects);
            }
            catch (Exception ex)
            {
                return ex.Message;
            }

            // create new build for each project. save in db
            // map assemblies
            // here we have to bind test assemblies with Builds
            // save test assembly in db

            // map suites
            // here we have to bind suites with assemblies 
            // save suites in db

            // map cases
            // bind cases with suites
            // save cases in db

            // map screenshots
            // bind screenshots with cases
            // save cases in db


            return "";
        }

        private string ValidateImportedConfigObjects(TestImportConfig importedConfig)
        {
            var mapper = new ConfigMapper();
            var mappedProjects = mapper.GetProjects(importedConfig.Projects).ToList();

            // validate projects
            var validationMessage = ConfigValidator.ValidateProjects(mappedProjects);
            if (validationMessage != "")
                return validationMessage;

            // validate test assemblies
            foreach (var project in importedConfig.Projects)
            {
                var assemblies = mapper.GetAssemblies(project.TestAssemblies);
                validationMessage = ConfigValidator.ValidateTestAssemblies(assemblies.ToList());

                if (validationMessage != "")
                    return validationMessage;
            }


            foreach (var project in importedConfig.Projects)
            {
                //validate suites
                foreach (var testAssemblyConfig in project.TestAssemblies)
                {
                    var items = mapper.GetSuites(testAssemblyConfig.TestSuites);
                    validationMessage = ConfigValidator.ValidateTestSuites(items.ToList());

                    if (validationMessage != "")
                        return validationMessage;


                    //validate casess
                    foreach (var testSuite in testAssemblyConfig.TestSuites)
                    {
                        var cases = mapper.GetCases(testSuite.TestCases);
                        validationMessage = ConfigValidator.ValidateTestCases(cases.ToList());

                        if (validationMessage != "")
                            return validationMessage;


                        //validate screenshots
                        foreach (var testCase in testSuite.TestCases)
                        {
                            validationMessage = ConfigValidator.ValidateScreenshots(testCase.ScreenShots);

                            if (validationMessage != "")
                                return validationMessage;
                        }
                    }
                }
            }

            return "";
        }


        private List<Project> BindImportedProjectWithExistedInDB(List<Project> mappedProjects)
        {
            for (int index = 0; index < mappedProjects.Count(); index++)
            {
                var project = mappedProjects[index];

                var projectName = project.Name.Trim();
                var projectsFromDb = ProjectRepository.Instance.Find(projectName);

                if (projectsFromDb.Count() == 0 || projectsFromDb.Count() > 1)
                    throw new Exception(new ErrorMessage()
                    {
                        Code = ErrorMessage.ErorCode.ObjectNotFoundInDb,
                        ExceptionMessage = $"Project {projectName} wasn't found in database (or it's more than 1 of them)"
                    }.ToString());

                project = projectsFromDb.FirstOrDefault();
            }

            return mappedProjects;
        }


        /* private List<TestAssembly> BindTestAssembliesWithBuilds()
        {
        }
*/
        // TODO: implement correct bindig between instances (parentObjId)

        // TODO: implement validation after config import and before store it in DB
    }
}