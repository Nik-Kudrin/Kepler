using Kepler.Common.DB;
using Kepler.Common.Models;

namespace Kepler.Common.Repository
{
    public class TestAssemblyRepository : BuildObjectRepository<TestAssembly>
    {
        private static TestAssemblyRepository _repoInstance;

        public static TestAssemblyRepository Instance
        {
            get
            {
                if (_repoInstance == null)
                {
                    var dbContext = new KeplerDataContext();
                    _repoInstance = new TestAssemblyRepository(dbContext);
                }

                return _repoInstance;
            }
        }


        private TestAssemblyRepository(KeplerDataContext dbContext) : base(dbContext, dbContext.TestAssemblies)
        {
        }
    }
}