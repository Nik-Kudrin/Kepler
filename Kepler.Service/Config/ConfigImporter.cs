using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Kepler.Common.Error;
using Kepler.Core;
using Kepler.Core.Common;
using Kepler.Models;
using Kepler.Service.Config;
using Newtonsoft.Json;

namespace Kepler.Service
{
    public class ConfigImporter
    {
        private ConfigMapper mapper = new ConfigMapper();

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

            var mappedProjects = mapper.GetProjects(importedConfig.Projects).ToList();

            try
            {
                mappedProjects = BindImportedProjectWithExistedInDB(mappedProjects);
            }
            catch (Exception ex)
            {
                return ex.Message;
            }

            var builds = BindImportedProjectWithBuilds(mappedProjects);
            var assemblies = BindTestAssembliesWithBuilds(importedConfig, mappedProjects);
            assemblies = BindTestSuitesWithAssemblies(importedConfig, builds, assemblies);


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


                    //validate cases
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

        /// <summary>
        /// Create new build for each project and save them in DB
        /// </summary>
        /// <param name="mappedProjects"></param>
        /// <returns>List new bounded builds</returns>
        private List<Build> BindImportedProjectWithBuilds(List<Project> mappedProjects)
        {
            var builds = new List<Build>();

            // create new build for each project. save in db
            foreach (var mappedProject in mappedProjects)
            {
                var build = new Build() {Status = ObjectStatus.InQueue};

                build = BuildRepository.Instance.SaveAndFlushChanges(build);
                mappedProject.Builds.Add(build.Id, build);
                mappedProject.LatestBuildId = build.Id;

                ProjectRepository.Instance.FlushChanges();
            }

            return builds;
        }

        /// <summary>
        /// Create new test assembly. Bind assembly with corresponding build and project. Save assembly in DB
        /// </summary>
        /// <param name="importedConfig"></param>
        /// <param name="mappedProjects"></param>
        /// <returns>List of new bounded assemblies</returns>
        private List<TestAssembly> BindTestAssembliesWithBuilds(TestImportConfig importedConfig, IEnumerable<Project> mappedProjects)
        {
            var assemblies = new List<TestAssembly>();

            foreach (var project in importedConfig.Projects)
            {
                var mappedAssemblies = mapper.GetAssemblies(project.TestAssemblies).ToList();

                for (int index = 0; index < mappedAssemblies.Count(); index++)
                {
                    var mappedAssembly = mappedAssemblies[0];
                    var assemblyProject = mappedProjects.FirstOrDefault(proj => proj.Name == project.Name);
                    mappedAssembly.ParentObjId = assemblyProject.Id;
                    mappedAssembly.BuildId = assemblyProject.LatestBuildId;
                    mappedAssembly.Status = ObjectStatus.InQueue;

                    mappedAssembly = TestAssemblyRepository.Instance.SaveAndFlushChanges(mappedAssembly);
                }

                assemblies.AddRange(mappedAssemblies);
            }

            return assemblies;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="importedConfig"></param>
        /// <param name="builds"></param>
        /// <param name="assemblies"></param>
        /// <returns></returns>
        private List<TestAssembly> BindTestSuitesWithAssemblies(TestImportConfig importedConfig, List<Build> builds, List<TestAssembly> assemblies)
        {
            // map suites
            // here we have to bind suites with assemblies 
            // save suites in db

            foreach (var projectConfig in importedConfig.Projects)
            {
                foreach (var testConfigAssembly in projectConfig.TestAssemblies)
                {
                    var mappedSuites = mapper.GetSuites(testConfigAssembly.TestSuites).ToList();
                    var currentAssembly = assemblies.Find(item => item.Name == testConfigAssembly.Name);

                    for (int index = 0; index < mappedSuites.Count(); index ++)
                    {
                        var suite = mappedSuites[index];

                        suite.BuildId = currentAssembly.BuildId;
                        suite.ParentObjId = currentAssembly.Id;
                        suite.Status = ObjectStatus.InQueue;
                    }

                    currentAssembly.TestSuites = mappedSuites.ToDictionary(item => item.Id, item => item);
                }
            }

            return assemblies;
        }
    }
}