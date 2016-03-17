using System.Collections.Generic;
using System.Linq;
using Kepler.Common.Models.Common;
using Kepler.Common.Repository;

namespace Kepler.Common.Models
{
    public class TestAssembly : BuildObject
    {
        public Dictionary<long, TestSuite> TestSuites { get; set; }

        public TestAssembly()
        {
        }

        public TestAssembly(string Name) : base(Name)
        {
            TestSuites = new Dictionary<long, TestSuite>();
        }

        public void InitChildObjectsFromDb(RepositoriesContainer repoContainer)
        {
            TestSuites = base.InitChildObjectsFromDb<TestSuiteRepository, TestSuite>(repoContainer.SuiteRepo);
            TestSuites.Values.ToList().ForEach(item => item.InitChildObjectsFromDb(repoContainer));
        }
    }
}