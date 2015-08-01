using Kepler.Models;

namespace Kepler.Core
{
    public class TestCaseRepository : BaseRepository<TestCase>
    {
        private static TestCaseRepository _repoInstance;

        public static TestCaseRepository Instance
        {
            get
            {
                if (_repoInstance == null)
                {
                    var dbContext = new KeplerDataContext();
                    _repoInstance = new TestCaseRepository(dbContext);
                }

                return _repoInstance;
            }
        }


        private TestCaseRepository(KeplerDataContext dbContext) : base(dbContext, dbContext.TestCases)
        {
        }
    }
}