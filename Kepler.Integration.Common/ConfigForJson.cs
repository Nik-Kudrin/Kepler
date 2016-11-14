using System.Configuration;

namespace Kepler.Integration.Common
{
    public class ConfigForJson
    {
        public string JsonResultFileName { get; set; }
        public string ProjectName { get; set; }
        public string BranchName { get; set; }
        public string BuildId { get; set; }

        public ConfigForJson()
        {
            ProjectName = ConfigurationManager.AppSettings["projectName"];
            BranchName = ConfigurationManager.AppSettings["branchName"];
            JsonResultFileName = ConfigurationManager.AppSettings["jsonResult"];
            BuildId = ConfigurationManager.AppSettings["buildId"];
        }
    }
}