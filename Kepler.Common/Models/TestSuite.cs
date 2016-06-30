using System.Collections.Generic;
using System.Linq;
using Kepler.Common.Models.Common;
using Kepler.Common.Repository;

namespace Kepler.Common.Models
{
    public class TestSuite : BuildObject
    {
        public Dictionary<long, TestCase> TestCases { get; set; }

        public TestSuite()
        {
        }

        public TestSuite(string Name) : base(Name)
        {
            TestCases = new Dictionary<long, TestCase>();
        }

        public void InitChildObjectsFromDb(RepositoriesContainer repoContainer)
        {
            TestCases =
                base.InitChildObjectsFromDb<TestCaseRepository, TestCase>(repoContainer.CaseRepo);

            TestCases.Values.ToList().ForEach(item => item.InitChildObjectsFromDb(repoContainer));
        }
    }
}