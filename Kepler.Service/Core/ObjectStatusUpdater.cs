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

        /// <summary>
        /// Set status for all objects in sub tree of element with provided ObjectId and Type
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
            List<ScreenShot> affectedScreenShots = null;

            if (typeof (TEntityBase) == typeof (Build))
            {
                if (updateParentObj)
                {
                    SetParentObjStatus<BuildRepository, Build>(BuildRepository.Instance, objectId, newStatus);
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

                return new List<ScreenShot>() {ScreenShotRepository.Instance.Get(objectId)};
            }

            return new List<ScreenShot>();
        }

        private static void SetParentObjStatus<T, TEntity>(T objectRepository, long objectId,
            ObjectStatus newStatus)
            where T : BaseRepository<TEntity>
            where TEntity : BuildObject
        {
            var item = objectRepository.Get(objectId);

            if (item == null) return;

            item.Status = newStatus;
            objectRepository.UpdateAndFlashChanges(item);
        }


        private static List<TEntityChild> SetChildObjStatuses<T, TEntityChild>(T childObjectRepository,
            long parentObjId,
            ObjectStatus newStatus)
            where T : BaseRepository<TEntityChild>
            where TEntityChild : BuildObject
        {
            var childItems = childObjectRepository.Find(item => item.ParentObjId == parentObjId).ToList();

            childItems.ForEach(item => item.Status = newStatus);
            childObjectRepository.UpdateAndFlashChanges(childItems);

            return childItems.ToList();
        }


        public static List<ScreenShot> SetObjectsStatus(string typeName, long objId, ObjectStatus newStatus)
        {
            switch (typeName.ToLowerInvariant())
            {
                case "build":
                    return RecursiveSetObjectsStatus<Build>(objId, newStatus);
                case "testAssembly":
                    return RecursiveSetObjectsStatus<TestAssembly>(objId, newStatus);
                case "testSuite":
                    return RecursiveSetObjectsStatus<TestSuite>(objId, newStatus);
                case "testCase":
                    return RecursiveSetObjectsStatus<TestCase>(objId, newStatus);
                case "screenShot":
                    return RecursiveSetObjectsStatus<ScreenShot>(objId, newStatus);

                default:
                    throw new ArgumentException(
                        $"TypeName {typeName} is not recognized. Possible values: build, testCase, testSuite, testAssembly, screenShot");
            }
        }
    }
}