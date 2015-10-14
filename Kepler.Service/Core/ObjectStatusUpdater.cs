using System;

namespace Kepler.Service.Core
{
    public class ObjectStatusUpdater
    {
        public static void UpdateAllObjectStatusesRecursively()
        {
            UpdateTestCasesStatuses();
            UpdateTestSuitesStatuses();
            UpdateTestAssembliesStatuses();
            UpdateBuildStatuses();
        }

        public static void UpdateTestCasesStatuses()
        {
            throw new NotImplementedException();

            // select all In progress, In queue Test Cases
            // select In queue, In progress screenshots
            // if > 0 in progress screenshtos, then set status = In progress .
            // Then return - exit

            // if at least 1 screenshot failed - set status = Failed
            // if all screenshots passed - set status Passed
        }

        public static void UpdateTestSuitesStatuses()
        {
            // select all In progress, In queue Test Suites
            // select all related test cases for suite

            // if > 0 in progress test cases, then set status = In progress .
            // Then return - exit

            // if at least 1 test case failed - set status = Failed
            // if all cases passed - set status Passed
        }

        public static void UpdateTestAssembliesStatuses()
        {
            // select all In progress, In queue Test Assemblies
            // select all related test suites for assembly

            // if > 0 in progress suite, then set status = In progress .
            // Then return - exit

            // if at least 1 suite failed - set status = Failed
            // if all suites passed - set status Passed
        }

        public static void UpdateBuildStatuses()
        {
            // select all In progress , In queue Builds

            // select all related test assembly for build

            // if > 0 in progress assembly, then set status = In progress .
            // Then return - exit

            // if at least 1 assembly failed - set status = Failed
            // if all assemblies passed - set status Passed
        }
    }
}