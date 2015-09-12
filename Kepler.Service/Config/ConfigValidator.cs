using System.Collections.Generic;
using System.Linq;
using Kepler.Common.Error;
using Kepler.Core;
using Kepler.Models;

namespace Kepler.Service.Config
{
    public static class ConfigValidator
    {
        private static string NameValidator<T>(ErrorMessage.ErorCode errorCode, List<T> configObjects) where T : InfoObject
        {
            if (configObjects.Select(item => item.Name.Trim()).Any(itemName => itemName == ""))
            {
                return (new ErrorMessage()
                {
                    Code = errorCode,
                    ExceptionMessage = ""
                }.ToString());
            }

            return "";
        }


        public static string ValidateProjects(List<Project> projects)
        {
            return NameValidator<Project>(ErrorMessage.ErorCode.ProjectDontHaveAName, projects);
        }

        public static string ValidateTestAssemblies(List<TestAssembly> assemblies)
        {
            return NameValidator<TestAssembly>(ErrorMessage.ErorCode.AssemblyDontHaveAName, assemblies);
        }

        public static string ValidateTestSuites(List<TestSuite> suites)
        {
            return NameValidator<TestSuite>(ErrorMessage.ErorCode.SuiteDontHaveAName, suites);
        }

        public static string ValidateTestCases(List<TestCase> cases)
        {
            return NameValidator<TestCase>(ErrorMessage.ErorCode.CaseDontHaveAName, cases);
        }

        public static string ValidateScreenshots(List<ScreenShot> screenShots)
        {
            if (screenShots.Select(item => item.ImagePath.Trim()).Any(screenShotPath => screenShotPath == ""))
            {
                return (new ErrorMessage()
                {
                    Code = ErrorMessage.ErorCode.ScreenShotHasEmptyFilePath,
                    ExceptionMessage = ""
                }.ToString());
            }

            return "";
        }
    }
}