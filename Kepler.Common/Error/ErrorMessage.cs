using System;

namespace Kepler.Common.Error
{
    public class ErrorMessage
    {
        public enum ErorCode
        {
            ParsingFileError,
            ProjectDontHaveAName,
            AssemblyDontHaveAName,
            SuiteDontHaveAName,
            CaseDontHaveAName,
            ScreenShotHasEmptyFilePath,
            ObjectNotFoundInDb
        }

        public ErorCode Code { get; set; }
        public string ExceptionMessage { get; set; }

        public override string ToString()
        {
            var codeMessage = string.Empty;

            switch (Code)
            {
                case ErorCode.ParsingFileError:
                    codeMessage = "Config file parsing error.";
                    break;
                case ErorCode.ProjectDontHaveAName:
                    codeMessage = "Project don't have a name";
                    break;
                case ErorCode.AssemblyDontHaveAName:
                    codeMessage = "TestAssembly don't have a name";
                    break;
                case ErorCode.SuiteDontHaveAName:
                    codeMessage = "TestSuite don't have a name";
                    break;
                case ErorCode.CaseDontHaveAName:
                    codeMessage = "TestCase don't have a name";
                    break;
                case ErorCode.ScreenShotHasEmptyFilePath:
                    codeMessage = "Screenshot has empty file path";
                    break;

                default:
                    throw new NotImplementedException("");
            }

            return $"Error: {codeMessage}. Exception: {ExceptionMessage}";
        }
    }
}