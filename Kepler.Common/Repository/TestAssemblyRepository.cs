using Kepler.Common.DB;
using Kepler.Common.Models;

namespace Kepler.Common.Repository
{
    public class TestAssemblyRepository : BuildObjectRepository<TestAssembly>
    {
        private static TestAssemblyRepository _repoInstance;

        public static TestAssemblyRepository Instance => new TestAssemblyRepository(new KeplerDataContext());

        private TestAssemblyRepository(KeplerDataContext dbContext) : base(dbContext, dbContext.TestAssemblies)
        {
        }
    }
}