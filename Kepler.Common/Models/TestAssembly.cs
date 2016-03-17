using System.Collections.Generic;
using System.Linq;
using Kepler.Common.Models.Common;
using Kepler.Common.Repository;

namespace Kepler.Common.Models
{
    public class TestAssembly : BuildObject /*, IChildInit*/
    {
        public Dictionary<long, TestSuite> TestSuites { get; set; }

        public TestAssembly()
        {
        }

        public TestAssembly(string Name) : base(Name)
        {
            TestSuites = new Dictionary<long, TestSuite>();
        }

        /*public void InitChildObjectsFromDb(TestSuiteRepository childObjectRepository)
        {
            TestSuites = base.InitChildObjectsFromDb<TestSuiteRepository, TestSuite>(childObjectRepository);
            TestSuites.Values.ToList().ForEach(item => item.InitChildObjectsFromDb());
        }*/

        /*public new void InitChildObjectsFromDb<T, TEntityChild>(T childObjectRepository)
            where T : BaseRepository<TEntityChild> where TEntityChild : BuildObject
        {
            TestSuites =
                base.InitChildObjectsFromDb<TestSuiteRepository, TestSuite>(childObjectRepository as TestSuiteRepository);

            var testCaseRepo = TestCaseRepository.Instance;
            TestSuites.Values.ToList()
                .ForEach(item => item.InitChildObjectsFromDb<TestCaseRepository, TestCase>(testCaseRepo));
        }*/

        public void InitChildObjectsFromDb(RepositoriesContainer repoContainer)
        {
            TestSuites = base.InitChildObjectsFromDb<TestSuiteRepository, TestSuite>(repoContainer.SuiteRepo);
            TestSuites.Values.ToList().ForEach(item => item.InitChildObjectsFromDb(repoContainer));

            /*TestSuites.Values.ToList()
                .ForEach(item => item.InitChildObjectsFromDb<TestCaseRepository, TestCase>(repoContainer.CaseRepo));*/
        }
    }
}