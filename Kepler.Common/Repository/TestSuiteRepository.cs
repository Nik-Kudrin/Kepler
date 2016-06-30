using Kepler.Common.Models;
using Kepler.Common.Models.Common;

namespace Kepler.Common.Repository
{
    public class TestSuiteRepository : BuildObjectRepository<TestSuite>, ICompleteObject<TestSuite>
    {
        public static TestSuiteRepository Instance => new TestSuiteRepository();

        private TestSuiteRepository()
        {
        }

        public TestSuite GetCompleteObject(RepositoriesContainer repoContainer, long id)
        {
            var suite = repoContainer.SuiteRepo.Get(id);
            suite.InitChildObjectsFromDb(repoContainer);

            return suite;
        }
    }
}