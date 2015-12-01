using Kepler.Common.DB;
using Kepler.Common.Models;

namespace Kepler.Common.Repository
{
    public class TestSuiteRepository : BuildObjectRepository<TestSuite>
    {
        private static TestSuiteRepository _repoInstance;

        public static TestSuiteRepository Instance => new TestSuiteRepository(new KeplerDataContext());

        private TestSuiteRepository(KeplerDataContext dbContext) : base(dbContext, dbContext.TestSuites)
        {
        }
    }
}