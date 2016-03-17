using Kepler.Common.DB;
using Kepler.Common.Models;
using Kepler.Common.Models.Common;

namespace Kepler.Common.Repository
{
    public class TestSuiteRepository : BuildObjectRepository<TestSuite>, ICompleteObject<TestSuite>
    {
        public static TestSuiteRepository Instance => new TestSuiteRepository(new KeplerDataContext());

        private TestSuiteRepository(KeplerDataContext dbContext) : base(dbContext, dbContext.TestSuites)
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