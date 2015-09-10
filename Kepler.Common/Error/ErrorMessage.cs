using System;

namespace Kepler.Common.Error
{
    public class ErrorMessage
    {
        public enum ErorCode
        {
            ParsingFileError,
            ProjectDontHaveAName
        }

        public ErorCode Code { get; set; }
        public string ExceptionMessage { get; set; }

        public override string ToString()
        {
            var codeMeassage = string.Empty;

            switch (Code)
            {
                case ErorCode.ParsingFileError:
                    codeMeassage = "Config file parsing error.";
                    break;

                case ErorCode.ProjectDontHaveAName:
                    codeMeassage = "Project don't have a name";
                    break;

                default:
                    throw new NotImplementedException("");
            }

            return $"Error: {codeMeassage}. Exception: {ExceptionMessage}";
        }
    }
}