using System.Collections.Generic;
using System.Linq;
using Kepler.Common.Models.Common;
using Kepler.Common.Repository;

namespace Kepler.Common.Models
{
    public class TestAssembly : BuildObject, IChildInit
    {
        public Dictionary<long, TestSuite> TestSuites { get; set; }

        public TestAssembly()
        {
        }

        public TestAssembly(string Name) : base(Name)
        {
            TestSuites = new Dictionary<long, TestSuite>();
        }

        public void InitChildObjectsFromDb()
        {
            TestSuites = InitChildObjectsFromDb<TestSuiteRepository, TestSuite>(TestSuiteRepository.Instance);
            TestSuites.Values.ToList().ForEach(item => item.InitChildObjectsFromDb());
        }
    }
}