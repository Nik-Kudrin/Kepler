using System.Collections.Generic;
using System.Linq;
using Kepler.Common.Error;
using Kepler.Common.Models;
using Kepler.Core;
using Kepler.Models;

namespace Kepler.Service.Config
{
    /// <summary>
    /// Validate imported JSON config
    /// </summary>
    public static class ConfigValidator
    {
        private static string NameValidator<T>(ErrorMessage.ErorCode errorCode, List<T> configObjects)
            where T : InfoObject
        {
            if (configObjects.Select(item => item.Name.Trim()).Any(itemName => itemName == ""))
            {
                return new ErrorMessage()
                {
                    Code = errorCode,
                    ExceptionMessage = ""
                }.ToString();
            }

            if (configObjects.Distinct().Count() != configObjects.Count)
            {
                return $"There are duplicated names in config for {typeof (T).Name}";
            }

            return "";
        }

        private static string EmptyListValidator<T>(ErrorMessage.ErorCode errorCode, List<T> configObjects)
            where T : InfoObject
        {
            if (configObjects == null || configObjects.Count == 0)
            {
                return new ErrorMessage()
                {
                    Code = ErrorMessage.ErorCode.EmptyListOfObjects,
                    ExceptionMessage = $"Object list of '{typeof (T).Name}' is empty"
                }.ToString();
            }

            return "";
        }


        public static string ValidateProjects(List<Project> projects)
        {
            var validationMessage = EmptyListValidator(ErrorMessage.ErorCode.ProjectDontHaveAName, projects);
            if (validationMessage != "")
                return validationMessage;

            return NameValidator(ErrorMessage.ErorCode.ProjectDontHaveAName, projects);
        }

        public static string ValidateBranches(List<Branch> branches)
        {
            var validationMessage = EmptyListValidator(ErrorMessage.ErorCode.BranchDontHaveAName, branches);
            if (validationMessage != "")
                return validationMessage;

            return NameValidator(ErrorMessage.ErorCode.BranchDontHaveAName, branches);
        }

        public static string ValidateTestAssemblies(List<TestAssembly> assemblies)
        {
            var validationMessage = EmptyListValidator(ErrorMessage.ErorCode.AssemblyDontHaveAName, assemblies);
            if (validationMessage != "")
                return validationMessage;

            return NameValidator(ErrorMessage.ErorCode.AssemblyDontHaveAName, assemblies);
        }

        public static string ValidateTestSuites(List<TestSuite> suites)
        {
            var validationMessage = EmptyListValidator(ErrorMessage.ErorCode.SuiteDontHaveAName, suites);
            if (validationMessage != "")
                return validationMessage;

            return NameValidator(ErrorMessage.ErorCode.SuiteDontHaveAName, suites);
        }

        public static string ValidateTestCases(List<TestCase> cases)
        {
            var validationMessage = EmptyListValidator(ErrorMessage.ErorCode.CaseDontHaveAName, cases);
            if (validationMessage != "")
                return validationMessage;

            return NameValidator(ErrorMessage.ErorCode.CaseDontHaveAName, cases);
        }

        public static string ValidateScreenshots(List<ScreenShot> screenShots)
        {
            var validationMessage = EmptyListValidator(ErrorMessage.ErorCode.ProjectDontHaveAName, screenShots);
            if (validationMessage != "")
                return validationMessage;

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