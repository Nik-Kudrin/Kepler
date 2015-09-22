using Kepler.Common.Core;
using Kepler.Models;

namespace Kepler.Core
{
    public class TestSuiteRepository : BuildObjectRepository<TestSuite>
    {
        private static TestSuiteRepository _repoInstance;

        public static TestSuiteRepository Instance
        {
            get
            {
                if (_repoInstance == null)
                {
                    var dbContext = new KeplerDataContext();
                    _repoInstance = new TestSuiteRepository(dbContext);
                }

                return _repoInstance;
            }
        }


        private TestSuiteRepository(KeplerDataContext dbContext) : base(dbContext, dbContext.TestSuites)
        {
        }
    }
}