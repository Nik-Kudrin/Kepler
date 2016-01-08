using Kepler.Common.DB;
using Kepler.Common.Models;

namespace Kepler.Common.Repository
{
    public class TestCaseRepository : BuildObjectRepository<TestCase>
    {
        public static TestCaseRepository Instance => new TestCaseRepository(new KeplerDataContext());

        private TestCaseRepository(KeplerDataContext dbContext) : base(dbContext, dbContext.TestCases)
        {
        }
    }
}