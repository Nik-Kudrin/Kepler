using System;
using System.Collections.Generic;
using System.Linq;
using Kepler.Common.Models;
using Kepler.Common.Models.Common;
using Kepler.Common.Repository;

namespace Kepler.Service.Core
{
    public class ObjectStatusUpdater
    {
        /// <summary>
        ///     Update StartDate, StopDate, Duration, PredictedDuration, based on changed build status
        /// </summary>
        /// <param name="buildId"></param>
        public static void UpdateBuildDurationFields(long buildId)
        {
            var build = BuildRepository.Instance.Get(buildId);
            if (build == null) return;

            switch (build.Status)
            {
                case ObjectStatus.InProgress:
                    // if StartDate already set, just get out of here
                    if (build.StartDate.HasValue) return;
                    SetBuildStartDate(build);

                    var builds = BuildRepository.Instance.Find(string.Format("WHERE BranchId = {0} AND Id <> {1}",
                        build.BranchId, buildId));


                    var averagePredictedBuildRunTicks = 0.0;

                    if (builds.Any())
                        averagePredictedBuildRunTicks = builds.Average(item => item.Duration?.Ticks ?? 0);

                    var longAverageTicks = Convert.ToInt64(averagePredictedBuildRunTicks);
                    build.PredictedDuration = TimeSpan.FromTicks(longAverageTicks);
                    build.PredictedDuration = new TimeSpan(build.PredictedDuration.Value.Days, build.PredictedDuration.Value.Hours, build.PredictedDuration.Value.Minutes, build.PredictedDuration.Value.Seconds);
                    break;

                case ObjectStatus.Failed:
                    SetBuildStartDate(build);

                    // if StopDate already set, just get out of here
                    if (build.StopDate.HasValue) return;

                    var numberNotProcessedScreenShots =
                        ScreenShotRepository.Instance.Find(
                            string.Format("WHERE BuildId = {0} AND (Status = {1} OR Status = {2})",
                                buildId, (int) ObjectStatus.InProgress, (int) ObjectStatus.InQueue)).Count();

                    // if build still in processing
                    if (numberNotProcessedScreenShots > 0) return;

                    UpdateBuildFailedScreenshotsNumber(build);

                    build.StopDate = DateTime.Now;
                    var duration = build.StopDate - build.StartDate;
                    build.Duration = new TimeSpan(duration.Value.Days, duration.Value.Hours, duration.Value.Minutes,
                        duration.Value.Seconds);

                    break;

                case ObjectStatus.Passed:
                    SetBuildStartDate(build);
                    // if StopDate already set, just get out of here
                    if (build.StopDate.HasValue) return;

                    build.StopDate = DateTime.Now;
                    duration = build.StopDate - build.StartDate;
                    build.Duration = new TimeSpan(duration.Value.Days, duration.Value.Hours, duration.Value.Minutes,
                        duration.Value.Seconds);
                    build.Duration = build.StopDate - build.StartDate;

                    break;

                case ObjectStatus.Stopped:
                    SetBuildStartDate(build);
                    // if StopDate already set, just get out of here
                    if (build.StopDate.HasValue) return;
                    UpdateBuildFailedScreenshotsNumber(build);
                    break;
            }

            BuildRepository.Instance.Update(build);
        }

        private static void SetBuildStartDate(Build build)
        {
            if (!build.StartDate.HasValue)
                build.StartDate = DateTime.Now;
        }

        private static void UpdateBuildFailedScreenshotsNumber(Build build)
        {
            build.StopDate = DateTime.Now;
            build.Duration = build.StopDate - build.StartDate;
            build.NumberFailedScreenshots = ScreenShotRepository.Instance.FindFailedInBuild(build.Id).Count();
        }

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
            where T : BaseObjRepository<TEntityBase>
            where M : BaseObjRepository<TEntityChild>
            where TEntityBase : BuildObject
            where TEntityChild : BuildObject
        {
            var baseItems =
                baseObjectRepository.Find(string.Format("WHERE Status = {0} OR Status = {1}",
                    (int) ObjectStatus.InQueue, (int) ObjectStatus.InProgress));

            foreach (var baseItem in baseItems)
            {
                var childItems = childObjectRepository.Find(new {ParentObjId = baseItem.Id});

                if (childItems.Any(item => item.Status == ObjectStatus.InProgress))
                {
                    baseItem.Status = ObjectStatus.InProgress;
                    baseObjectRepository.Update(baseItem);
                    continue;
                }

                // because we should set correct "stop" time for failed build
                if (childItems.Any(item => item.Status == ObjectStatus.Failed))
                {
                    if (typeof (TEntityBase) == typeof (Build))
                    {
                        var isSomeItemsInProgress = childItems.Any(item => item.Status == ObjectStatus.InQueue ||
                                                                           item.Status == ObjectStatus.InProgress);
                        if (!isSomeItemsInProgress)
                        {
                            baseItem.Status = ObjectStatus.Failed;
                            baseObjectRepository.Update(baseItem);
                            continue;
                        }
                    }

                    baseItem.Status = ObjectStatus.Failed;
                    baseObjectRepository.Update(baseItem);
                    continue;
                }

                var successItemsCount = childItems.Count(item => item.Status == ObjectStatus.Stopped ||
                                                                 item.Status == ObjectStatus.Passed);
                if (successItemsCount == childItems.Count())
                {
                    baseItem.Status = ObjectStatus.Passed;
                }

                baseObjectRepository.Update(baseItem);
            }

            if (typeof (TEntityBase) != typeof (Build)) return;
            foreach (var baseItem in baseItems)
            {
                UpdateBuildDurationFields(baseItem.Id);
            }
        }


        /// <summary>
        ///     Set status for all objects in sub tree of element with provided ObjectId and Type
        /// </summary>
        /// <typeparam name="TEntityBase"></typeparam>
        /// <param name="objectId"></param>
        /// <param name="newStatus"></param>
        /// <param name="updateParentObj"></param>
        /// <returns>Collection of affected screenshots, status of which were updated</returns>
        public static List<ScreenShot> RecursiveSetObjectsStatus<TEntityBase>(long objectId,
            ObjectStatus newStatus,
            bool updateParentObj = true)
            where TEntityBase : BuildObject
        {
            var affectedScreenShots = new List<ScreenShot>();

            if (typeof (TEntityBase) == typeof (Build))
            {
                if (updateParentObj)
                {
                    SetParentObjStatus<BuildRepository, Build>(BuildRepository.Instance, objectId, newStatus);
                    UpdateBuildDurationFields(objectId);
                }
                var childObjects =
                    SetChildObjStatuses<TestAssemblyRepository, TestAssembly>(TestAssemblyRepository.Instance,
                        objectId, newStatus);

                foreach (var child in childObjects)
                {
                    affectedScreenShots.AddRange(RecursiveSetObjectsStatus<TestAssembly>(child.Id, newStatus, false));
                }
            }
            else if (typeof (TEntityBase) == typeof (TestAssembly))
            {
                if (updateParentObj)
                {
                    SetParentObjStatus<TestAssemblyRepository, TestAssembly>(TestAssemblyRepository.Instance, objectId,
                        newStatus);
                }
                var childObjects = SetChildObjStatuses<TestSuiteRepository, TestSuite>(TestSuiteRepository.Instance,
                    objectId, newStatus);

                foreach (var child in childObjects)
                {
                    affectedScreenShots.AddRange(RecursiveSetObjectsStatus<TestSuite>(child.Id, newStatus, false));
                }
            }
            else if (typeof (TEntityBase) == typeof (TestSuite))
            {
                if (updateParentObj)
                {
                    SetParentObjStatus<TestSuiteRepository, TestSuite>(TestSuiteRepository.Instance, objectId, newStatus);
                }
                var childObjects = SetChildObjStatuses<TestCaseRepository, TestCase>(TestCaseRepository.Instance,
                    objectId, newStatus);

                foreach (var child in childObjects)
                {
                    affectedScreenShots.AddRange(RecursiveSetObjectsStatus<TestCase>(child.Id, newStatus, false));
                }
            }
            else if (typeof (TEntityBase) == typeof (TestCase))
            {
                if (updateParentObj)
                {
                    SetParentObjStatus<TestCaseRepository, TestCase>(TestCaseRepository.Instance, objectId, newStatus);
                }

                return SetChildObjStatuses<ScreenShotRepository, ScreenShot>(ScreenShotRepository.Instance, objectId,
                    newStatus);
            }
            else if (typeof (TEntityBase) == typeof (ScreenShot))
            {
                SetParentObjStatus<ScreenShotRepository, ScreenShot>(ScreenShotRepository.Instance, objectId, newStatus);

                return new List<ScreenShot> {ScreenShotRepository.Instance.Get(objectId)};
            }

            return new List<ScreenShot>();
        }

        public static void SetParentObjStatus<T, TEntity>(T objectRepository, long objectId,
            ObjectStatus newStatus)
            where T : BaseObjRepository<TEntity>
            where TEntity : BuildObject
        {
            var item = objectRepository.Get(objectId);
            if (item == null) return;

            item.Status = newStatus;

            // update baseline for screenshot
            if (typeof (TEntity) == typeof (ScreenShot) && newStatus == ObjectStatus.Passed)
            {
                var screenShotRepo = ScreenShotRepository.Instance;
                var actualScreenShot = screenShotRepo.Get(item.Id);

                var oldScreenShot = screenShotRepo.GetBaselineScreenShot(actualScreenShot.BaseLineId,
                    actualScreenShot.Name);

                if (oldScreenShot != null)
                {
                    oldScreenShot.IsLastPassed = false;
                    screenShotRepo.Update(oldScreenShot);
                }

                actualScreenShot.IsLastPassed = true;
                actualScreenShot.Status = ObjectStatus.Passed;

                screenShotRepo.Update(actualScreenShot);
                return;
            }

            objectRepository.Update(item);
        }


        public static List<TEntityChild> SetChildObjStatuses<T, TEntityChild>(T childObjectRepository,
            long parentObjId,
            ObjectStatus newStatus)
            where T : BaseObjRepository<TEntityChild>
            where TEntityChild : BuildObject
        {
            if (typeof (TEntityChild) == typeof (ScreenShot) && newStatus == ObjectStatus.Passed)
            {
                var screenShotRepo = ScreenShotRepository.Instance;
                var childScreenShots = screenShotRepo.Find(new {ParentObjId = parentObjId});

                foreach (var screenShot in childScreenShots)
                {
                    SetParentObjStatus<ScreenShotRepository, ScreenShot>(screenShotRepo, screenShot.Id, newStatus);
                }

                return childObjectRepository.Find(new {ParentObjId = parentObjId}).ToList();
            }

            var childItems = childObjectRepository.Find(new {ParentObjId = parentObjId}).ToList();

            childItems.ForEach(item => item.Status = newStatus);
            childObjectRepository.Update(childItems);

            return childItems;
        }


        public static List<ScreenShot> SetObjectsStatus(string typeName, long objId, ObjectStatus newStatus)
        {
            switch (typeName.ToLowerInvariant())
            {
                case "build":
                    if (newStatus == ObjectStatus.InQueue)
                    {
                        var buildRepo = BuildRepository.Instance;

                        var build = buildRepo.Get(objId);
                        build.Duration = null;
                        build.StartDate = null;
                        build.StopDate = null;
                        build.PredictedDuration = null;
                        buildRepo.Update(build);
                    }

                    return RecursiveSetObjectsStatus<Build>(objId, newStatus);
                case "testassembly":
                    return RecursiveSetObjectsStatus<TestAssembly>(objId, newStatus);
                case "testsuite":
                    return RecursiveSetObjectsStatus<TestSuite>(objId, newStatus);
                case "testcase":
                    return RecursiveSetObjectsStatus<TestCase>(objId, newStatus);
                case "screenshot":
                    return RecursiveSetObjectsStatus<ScreenShot>(objId, newStatus);

                default:
                    throw new ArgumentException(
                        $"TypeName {typeName} is not recognized. Possible values: build, testCase, testSuite, testAssembly, screenShot");
            }
        }
    }
}