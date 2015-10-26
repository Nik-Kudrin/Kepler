using System;

namespace Kepler.Common.Error
{
    public class ErrorMessage
    {
        public enum ErorCode
        {
            ParsingFileError,

            ProjectDontHaveAName,
            ProjectDontHaveMainBranch,

            BranchDontHaveAName,
            AssemblyDontHaveAName,
            SuiteDontHaveAName,
            CaseDontHaveAName,
            ScreenShotHasEmptyFilePath,
            EmptyListOfObjects,

            UndefinedError,
            ObjectNotFoundInDb,
            NotUniqueObjects,

            AddTaskToImageWorkerError,
            ScreenShotDoesntHaveAName
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
                    codeMessage = "Project doesn't have a name";
                    break;
                case ErorCode.BranchDontHaveAName:
                    codeMessage = "Branch doesn't have a name";
                    break;
                case ErorCode.ProjectDontHaveMainBranch:
                    codeMessage = "Project doesn't have main branch";
                    break;
                case ErorCode.AssemblyDontHaveAName:
                    codeMessage = "TestAssembly doesn't have a name";
                    break;
                case ErorCode.SuiteDontHaveAName:
                    codeMessage = "TestSuite doesn't have a name";
                    break;
                case ErorCode.CaseDontHaveAName:
                    codeMessage = "TestCase doesn't have a name";
                    break;
                case ErorCode.ScreenShotDoesntHaveAName:
                    codeMessage = "ScreenShot doesn't have a name";
                    break;
                case ErorCode.ScreenShotHasEmptyFilePath:
                    codeMessage = "Screenshot has empty file path";
                    break;
                case ErorCode.ObjectNotFoundInDb:
                    codeMessage = "Object wasn't found in Database";
                    break;
                case ErorCode.NotUniqueObjects:
                    codeMessage = "Object is not unique in Database";
                    break;
                case ErorCode.EmptyListOfObjects:
                    codeMessage = "List of objects is empty";
                    break;
                case ErorCode.AddTaskToImageWorkerError:
                    codeMessage = "Error happend in process to add images for diff comparison";
                    break;
                case ErorCode.UndefinedError:
                    codeMessage = "Undefined error";
                    break;

                default:
                    return "Detailed text about this type of error isn't written";
            }

            return $"Error: {codeMessage}. {ExceptionMessage}";
        }
    }
}