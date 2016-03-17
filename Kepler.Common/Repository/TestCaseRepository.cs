using Kepler.Common.DB;
using Kepler.Common.Models;
using Kepler.Common.Models.Common;

namespace Kepler.Common.Repository
{
    public class TestCaseRepository : BuildObjectRepository<TestCase>, ICompleteObject<TestCase>
    {
        public static TestCaseRepository Instance => new TestCaseRepository(new KeplerDataContext());

        private TestCaseRepository(KeplerDataContext dbContext) : base(dbContext, dbContext.TestCases)
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