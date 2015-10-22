using System.Linq;
using Kepler.Common.Models.Common;
using Kepler.Common.Repository;

namespace Kepler.Service.Core
{
    public class ObjectStatusUpdater
    {
        public static void UpdateAllObjectStatusesToActual()
        {
            UpdateTestCasesStatuseToActual();
            UpdateTestSuitesStatusToActual();
            UpdateTestAssembliesStatusToActual();
            UpdateBuildsStatusToActual();
        }

        // select all In progress, In queue Test Cases
        // select In queue, In progress screenshots
        // if > 0 in progress screenshtos, then set status = In progress .
        // Then return - exit

        // if at least 1 screenshot failed - set status = Failed
        // if all screenshots passed - set status Passed
        public static void UpdateTestCasesStatuseToActual()
        {
            var cases = TestCaseRepository.Instance.Find(
                item => item.Status == ObjectStatus.InQueue || item.Status == ObjectStatus.InProgress);

            foreach (var testCase in cases)
            {
                var screenShots = ScreenShotRepository.Instance.Find(item => item.ParentObjId == testCase.Id);

                if (screenShots.Any(item => item.Status == ObjectStatus.Failed))
                {
                    testCase.Status = ObjectStatus.Failed;
                    continue;
                }

                if (screenShots.Any(item => item.Status == ObjectStatus.InProgress))
                {
                    testCase.Status = ObjectStatus.InProgress;
                    continue;
                }

                if (screenShots.All(item => item.Status == ObjectStatus.Passed))
                {
                    testCase.Status = ObjectStatus.Passed;
                }

                TestCaseRepository.Instance.Update(testCase);
            }

            TestCaseRepository.Instance.FlushChanges();
        }


        // select all In progress, In queue Test Suites
        // select all related test cases for suite

        // if > 0 in progress test cases, then set status = In progress .
        // Then return - exit

        // if at least 1 test case failed - set status = Failed
        // if all cases passed - set status Passed
        public static void UpdateTestSuitesStatusToActual()
        {
            var suites = TestSuiteRepository.Instance.Find(
                item => item.Status == ObjectStatus.InQueue || item.Status == ObjectStatus.InProgress);

            foreach (var suite in suites)
            {
                var cases = TestCaseRepository.Instance.Find(item => item.ParentObjId == suite.Id);

                if (cases.Any(item => item.Status == ObjectStatus.Failed))
                {
                    suite.Status = ObjectStatus.Failed;
                    continue;
                }

                if (cases.Any(item => item.Status == ObjectStatus.InProgress))
                {
                    suite.Status = ObjectStatus.InProgress;
                    continue;
                }

                if (cases.All(item => item.Status == ObjectStatus.Passed))
                {
                    suite.Status = ObjectStatus.Passed;
                }

                TestSuiteRepository.Instance.Update(suite);
            }

            TestSuiteRepository.Instance.FlushChanges();
        }

        // select all In progress, In queue Test Assemblies
        // select all related test suites for assembly

        // if > 0 in progress suite, then set status = In progress .
        // Then return - exit

        // if at least 1 suite failed - set status = Failed
        // if all suites passed - set status Passed
        public static void UpdateTestAssembliesStatusToActual()
        {
            var assemblies = TestAssemblyRepository.Instance.Find(
                item => item.Status == ObjectStatus.InQueue || item.Status == ObjectStatus.InProgress);

            foreach (var assembly in assemblies)
            {
                var suites = TestSuiteRepository.Instance.Find(item => item.ParentObjId == assembly.Id);

                if (suites.Any(item => item.Status == ObjectStatus.Failed))
                {
                    assembly.Status = ObjectStatus.Failed;
                    continue;
                }

                if (suites.Any(item => item.Status == ObjectStatus.InProgress))
                {
                    assembly.Status = ObjectStatus.InProgress;
                    continue;
                }

                if (suites.All(item => item.Status == ObjectStatus.Passed))
                {
                    assembly.Status = ObjectStatus.Passed;
                }

                TestAssemblyRepository.Instance.Update(assembly);
            }

            TestAssemblyRepository.Instance.FlushChanges();
        }

        // select all In progress , In queue Builds

        // select all related test assembly for build

        // if > 0 in progress assembly, then set status = In progress .
        // Then return - exit

        // if at least 1 assembly failed - set status = Failed
        // if all assemblies passed - set status Passed
        public static void UpdateBuildsStatusToActual()
        {
            var builds = BuildRepository.Instance.Find(
                item => item.Status == ObjectStatus.InQueue || item.Status == ObjectStatus.InProgress);

            foreach (var build in builds)
            {
                var assemblies = TestAssemblyRepository.Instance.Find(item => item.ParentObjId == build.Id);

                if (assemblies.Any(item => item.Status == ObjectStatus.Failed))
                {
                    build.Status = ObjectStatus.Failed;
                    continue;
                }

                if (assemblies.Any(item => item.Status == ObjectStatus.InProgress))
                {
                    build.Status = ObjectStatus.InProgress;
                    continue;
                }

                if (assemblies.All(item => item.Status == ObjectStatus.Passed))
                {
                    build.Status = ObjectStatus.Passed;
                }

                BuildRepository.Instance.Update(build);
            }

            BuildRepository.Instance.FlushChanges();
        }


        private void UpdateObjectStatusToActual<T, M, TEntity>(T baseObjectRepository, M childObjectRepository)
            where T : BaseRepository<TEntity>
            where M : BaseRepository<TEntity>
            where TEntity : BuildObject
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