using Kepler.Common.Core;
using Kepler.Models;

namespace Kepler.Core
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