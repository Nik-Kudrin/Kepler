using System.Collections.Generic;
using System.Linq;
using Kepler.Common.Models.Common;
using Kepler.Common.Repository;

namespace Kepler.Common.Models
{
    public class TestSuite : BuildObject /*, IChildInit*/
    {
        public Dictionary<long, TestCase> TestCases { get; set; }

        public TestSuite()
        {
        }

        public TestSuite(string Name) : base(Name)
        {
            TestCases = new Dictionary<long, TestCase>();
        }

        /*public void InitChildObjectsFromDb()
        {
            TestCases = InitChildObjectsFromDb<TestCaseRepository, TestCase>(TestCaseRepository.Instance);
            TestCases.Values.ToList().ForEach(item => item.InitChildObjectsFromDb());
        }*/

        /*public new void InitChildObjectsFromDb<T, TEntityChild>(T childObjectRepository)
            where T : BaseRepository<TEntityChild> where TEntityChild : BuildObject
        {
            TestCases =
                base.InitChildObjectsFromDb<TestCaseRepository, TestCase>(childObjectRepository as TestCaseRepository);

            var screenShotRepo = ScreenShotRepository.Instance;
            TestCases.Values.ToList()
                .ForEach(item => item.InitChildObjectsFromDb<ScreenShotRepository, ScreenShot>(screenShotRepo));
        }
*/

        public void InitChildObjectsFromDb(RepositoriesContainer repoContainer)
        {
            TestCases =
                base.InitChildObjectsFromDb<TestCaseRepository, TestCase>(repoContainer.CaseRepo);

            TestCases.Values.ToList().ForEach(item => item.InitChildObjectsFromDb(repoContainer));

            /*TestCases.Values.ToList()
                .ForEach(
                    item => item.InitChildObjectsFromDb<ScreenShotRepository, ScreenShot>(repoContainer.ScreenShotRepo));*/
        }
    }
}