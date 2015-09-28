using System;

namespace Kepler.Common.Error
{
    public class ErrorMessage
    {
        public enum ErorCode
        {
            ParsingFileError,
            ProjectDontHaveAName,
            BranchDontHaveAName,
            AssemblyDontHaveAName,
            SuiteDontHaveAName,
            CaseDontHaveAName,
            ScreenShotHasEmptyFilePath,
            ObjectNotFoundInDb,
            EmptyListOfObjects,
            ProjectDontHaveMainBranch,

            AddTaskToImageWorkerError
        }

        public ErorCode Code { get; set; }
        public string ExceptionMessage { get; set; }

        public override string ToString()
        {
            var codeMessage = string.Empty;

            switch (Code)
            {
                case ErorCode.ParsingFileError:
                    codeMessage = "Config file parsing error";
                    break;
                case ErorCode.ProjectDontHaveAName:
                    codeMessage = "Project don't have a name";
                    break;
                case ErorCode.BranchDontHaveAName:
                    codeMessage = "Branch don't have a name";
                    break;
                case ErorCode.ProjectDontHaveMainBranch:
                    codeMessage = "Project don't have main branch";
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
                case ErorCode.ObjectNotFoundInDb:
                    codeMessage = "Object wasn't found in Database";
                    break;
                case ErorCode.EmptyListOfObjects:
                    codeMessage = "List of objects is empty";
                    break;
                case ErorCode.AddTaskToImageWorkerError:
                    codeMessage = "Error happend in process to add images for diff comparison";
                    break;

                default:
                    throw new NotImplementedException("Detaileds text about this type of error code isn't written");
            }

            return $"Error: {codeMessage}. {ExceptionMessage}";
        }
    }
}