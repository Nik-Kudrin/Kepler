using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Kepler.Common.Error;
using Kepler.Common.Models;
using Kepler.Common.Models.Common;
using Kepler.Common.Repository;

namespace Kepler.Service.Core
{
    public class DataCleaner
    {
        public static void DeleteDirectory(string path)
        {
            try
            {
                Directory.Delete(path, true);
            }
            catch (Exception ex)
            {
                ErrorMessageRepository.Instance.Insert(new ErrorMessage() {ExceptionMessage = ex.Message});
            }
        }

        public static void DeleteObjectsTreeRecurively<TEntityBase>(long objectId, bool deleteDirectory = false)
            where TEntityBase : BuildObject
        {
            if (typeof (TEntityBase) == typeof (Project))
            {
                var childObjects = GetChildObjects<BranchRepository, Branch>(BranchRepository.Instance, objectId);

                foreach (var child in childObjects)
                {
                    DeleteObjectsTreeRecurively<TestAssembly>(child.Id);
                }
            }
            else if (typeof (TEntityBase) == typeof (Branch))
            {
                var childObjects =
                    SetChildObjStatuses<TestAssemblyRepository, TestAssembly>(TestAssemblyRepository.Instance,
                        objectId, );

                if (deleteDirectory)
                    DeleteDirectory();

                foreach (var child in childObjects)
                {
                    DeleteObjectsTreeRecurively<TestAssembly>(child.Id);
                }
            }
            else if (typeof (TEntityBase) == typeof (Build))
            {
                var childObjects =
                    SetChildObjStatuses<TestAssemblyRepository, TestAssembly>(TestAssemblyRepository.Instance,
                        objectId, );

                foreach (var child in childObjects)
                {
                    DeleteObjectsTreeRecurively<TestAssembly>(child.Id);
                }
            }
            else if (typeof (TEntityBase) == typeof (TestAssembly))
            {
                var childObjects = SetChildObjStatuses<TestSuiteRepository, TestSuite>(TestSuiteRepository.Instance,
                    objectId, );

                foreach (var child in childObjects)
                {
                    DeleteObjectsTreeRecurively<TestSuite>(child.Id);
                }
            }
            else if (typeof (TEntityBase) == typeof (TestSuite))
            {
                var childObjects = SetChildObjStatuses<TestCaseRepository, TestCase>(TestCaseRepository.Instance,
                    objectId, );

                foreach (var child in childObjects)
                {
                    DeleteObjectsTreeRecurively<TestCase>(child.Id);
                }
            }
            else if (typeof (TEntityBase) == typeof (TestCase))
            {
                return SetChildObjStatuses<ScreenShotRepository, ScreenShot>(ScreenShotRepository.Instance, objectId, 
                    );
            }
            else if (typeof (TEntityBase) == typeof (ScreenShot))
            {
                SetParentObjStatus<ScreenShotRepository, ScreenShot>(ScreenShotRepository.Instance, objectId, );

                return new List<ScreenShot>() {ScreenShotRepository.Instance.Get(objectId)};
            }
        }


        private static List<TEntityChild> GetChildObjects<T, TEntityChild>(T childObjectRepository, long parentObjId)
            where T : BaseRepository<TEntityChild>
            where TEntityChild : BuildObject
        {
            return childObjectRepository.Find(item => item.ParentObjId == parentObjId).ToList();
        }
    }
}