using System.Collections.Generic;
using Kepler.Core;


namespace Kepler.Service
{
    public class TestImportConfig
    {
        public List<ProjectConfig> Projects { get; set; }

        public class BaseConfig
        {
            public string Name { get; set; }
        }

        public class ProjectConfig : BaseConfig
        {
            public List<BranchConfig> Branches { get; set; }
        }

        public class BranchConfig : BaseConfig
        {
            public List<TestAssemblyConfig> TestAssemblies { get; set; }
        }


        public class TestAssemblyConfig : BaseConfig
        {
            public List<TestSuiteConfig> TestSuites { get; set; }
        }

        public class TestSuiteConfig : BaseConfig
        {
            public List<TestCaseConfig> TestCases { get; set; }
        }

        public class TestCaseConfig : BaseConfig
        {
            public List<ScreenShot> ScreenShots { get; set; }
        }
    }
}