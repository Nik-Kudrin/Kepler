using System.Linq;
using Kepler.Common.Models;
using Kepler.Common.Models.Common;
using Kepler.Common.Repository;

namespace Kepler.Service.Core
{
    public class ObjectStatusUpdater
    {
        // select all In progress, In queue Test Cases
        // select In queue, In progress screenshots
        // if > 0 in progress screenshtos, then set status = In progress .
        // Then return - exit

        // if at least 1 screenshot failed - set status = Failed
        // if all screenshots passed - set status Passed

        public static void UpdateAllObjectStatusesToActual()
        {
            UpdateObjectStatusToActual<TestCaseRepository, ScreenShotRepository, TestCase, ScreenShot>(
                TestCaseRepository.Instance,
                ScreenShotRepository.Instance);

            UpdateObjectStatusToActual<TestSuiteRepository, TestCaseRepository, TestSuite, TestCase>(
                TestSuiteRepository.Instance,
                TestCaseRepository.Instance);

            UpdateObjectStatusToActual<TestAssemblyRepository, TestSuiteRepository, TestAssembly, TestSuite>(
                TestAssemblyRepository.Instance,
                TestSuiteRepository.Instance);

            UpdateObjectStatusToActual<BuildRepository, TestAssemblyRepository, Build, TestAssembly>(
                BuildRepository.Instance,
                TestAssemblyRepository.Instance);
        }


        private static void UpdateObjectStatusToActual<T, M, TEntityBase, TEntityChild>(T baseObjectRepository,
            M childObjectRepository)
            where T : BaseRepository<TEntityBase>
            where M : BaseRepository<TEntityChild>
            where TEntityBase : BuildObject
            where TEntityChild : BuildObject
        {
            var baseItems =
                baseObjectRepository.Find(item => item.Status == ObjectStatus.InQueue ||
                                                  item.Status == ObjectStatus.InProgress);

            foreach (var baseItem in baseItems)
            {
                var childItems = childObjectRepository.Find(item => item.ParentObjId == baseItem.Id);

                if (childItems.Any(item => item.Status == ObjectStatus.Failed))
                {
                    baseItem.Status = ObjectStatus.Failed;
                    continue;
                }

                if (childItems.Any(item => item.Status == ObjectStatus.InProgress))
                {
                    baseItem.Status = ObjectStatus.InProgress;
                    continue;
                }

                if (childItems.All(item => item.Status == ObjectStatus.Passed))
                {
                    baseItem.Status = ObjectStatus.Passed;
                }

                baseObjectRepository.Update(baseItem);
            }

            baseObjectRepository.FlushChanges();
        }
    }
}