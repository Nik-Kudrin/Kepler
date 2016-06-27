using Kepler.Common.Models;
using Kepler.Common.Models.Common;

namespace Kepler.Common.Repository
{
    public class TestCaseRepository : BuildObjectRepository<TestCase>, ICompleteObject<TestCase>
    {
        public static TestCaseRepository Instance => new TestCaseRepository();

        private TestCaseRepository()
        {
        }

        public TestCase GetCompleteObject(RepositoriesContainer repoContainer, long id)
        {
            var testCase = repoContainer.CaseRepo.Get(id);
            testCase.InitChildObjectsFromDb(repoContainer);

            return testCase;
        }
    }
}