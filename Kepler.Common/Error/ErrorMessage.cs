using System;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Runtime.Serialization;
using System.ServiceModel.Web;

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

            ObjectNotFoundInDb,
            NotUniqueObjects,

            RunOperationError,
            SetObjectStatusError,

            AddTaskToImageWorkerError,
            ScreenShotDoesntHaveAName,

            UndefinedError
        }

        public ErrorMessage()
        {
            Code = ErorCode.UndefinedError;
            Time = DateTime.Now;
        }

        [DataMember]
        [Key]
        public long Id { get; set; }

        [DataMember]
        [DataType(DataType.DateTime)]
        public DateTime? Time { get; set; }

        [DataMember]
        public ErorCode Code { get; set; }

        [DataMember]
        public bool IsLastViewed { get; set; }

        private string _exceptionMessage;

        [DataMember]
        public string ExceptionMessage
        {
            get { return ToString(); }
            set { _exceptionMessage = value; }
        }


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
                case ErorCode.RunOperationError:
                    codeMessage = "Error happend during run operation";
                    break;
                case ErorCode.SetObjectStatusError:
                    codeMessage = "Error happend during set status for object";
                    break;
                case ErorCode.UndefinedError:
                    codeMessage = "Undefined error";
                    break;

                default:
                    return "Detailed text about this type of error isn't written";
            }

            return $"Error: {codeMessage}. {_exceptionMessage}";
        }

        public WebFaultException<string> ConvertToWebFaultException(HttpStatusCode statusCode)
        {
            return new WebFaultException<string>(ToString(), statusCode);
        }
    }
}