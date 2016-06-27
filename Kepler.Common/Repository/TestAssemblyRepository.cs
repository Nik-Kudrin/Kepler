using Kepler.Common.Models;
using Kepler.Common.Models.Common;

namespace Kepler.Common.Repository
{
    public class TestAssemblyRepository : BuildObjectRepository<TestAssembly>, ICompleteObject<TestAssembly>
    {
        public static TestAssemblyRepository Instance => new TestAssemblyRepository();

        private TestAssemblyRepository()
        {
        }

        public TestAssembly GetCompleteObject(RepositoriesContainer repoContainer, long id)
        {
            var assembly = repoContainer.AssemblyRepo.Get(id);
            assembly.InitChildObjectsFromDb(repoContainer);

            return assembly;
        }
    }
}