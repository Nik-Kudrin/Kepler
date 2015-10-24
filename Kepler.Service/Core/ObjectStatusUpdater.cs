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

        public static void RecursiveSetObjectsStatus<TEntityBase>(long objectId, ObjectStatus newStatus,
            bool updateParentObj = true)
            where TEntityBase : BuildObject
        {
            if (typeof (TEntityBase) == typeof (Build))
            {
                if (updateParentObj)
                {
                    SetParentObjStatus<BuildRepository, Build>(BuildRepository.Instance, objectId, newStatus);
                }
                var childObjects =
                    SetChildObjStatuses<TestAssemblyRepository, TestAssembly>(TestAssemblyRepository.Instance,
                        objectId, newStatus);

                childObjects.ForEach(item => RecursiveSetObjectsStatus<TestAssembly>(item.Id, newStatus, false));
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

                childObjects.ForEach(item => RecursiveSetObjectsStatus<TestSuite>(item.Id, newStatus, false));
            }
            else if (typeof (TEntityBase) == typeof (TestSuite))
            {
                if (updateParentObj)
                {
                    SetParentObjStatus<TestSuiteRepository, TestSuite>(TestSuiteRepository.Instance, objectId, newStatus);
                }
                var childObjects = SetChildObjStatuses<TestCaseRepository, TestCase>(TestCaseRepository.Instance,
                    objectId, newStatus);

                childObjects.ForEach(item => RecursiveSetObjectsStatus<TestCase>(item.Id, newStatus, false));
            }
            else if (typeof (TEntityBase) == typeof (TestCase))
            {
                if (updateParentObj)
                {
                    SetParentObjStatus<TestCaseRepository, TestCase>(TestCaseRepository.Instance, objectId, newStatus);
                }
                SetChildObjStatuses<ScreenShotRepository, ScreenShot>(ScreenShotRepository.Instance, objectId, newStatus);
            }
            else if (typeof (TEntityBase) == typeof (ScreenShot))
            {
                SetParentObjStatus<ScreenShotRepository, ScreenShot>(ScreenShotRepository.Instance, objectId, newStatus);
            }
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
    }
}