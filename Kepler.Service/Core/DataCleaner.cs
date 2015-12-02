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
                childObjects.ForEach(child => DeleteObjectsTreeRecurively<TestAssembly>(child.Id));

                var parentObjRepo = ProjectRepository.Instance;
                parentObjRepo.Remove(parentObjRepo.Get(objectId));
            }
            else if (typeof (TEntityBase) == typeof (Branch))
            {
                var childObjects = GetChildObjects<BuildRepository, Build>(BuildRepository.Instance, objectId);

                if (deleteDirectory)
                    DeleteDirectory();

                childObjects.ForEach(child => DeleteObjectsTreeRecurively<TestAssembly>(child.Id));

                var parentObjRepo = BranchRepository.Instance;
                parentObjRepo.Remove(parentObjRepo.Get(objectId));
            }
            else if (typeof (TEntityBase) == typeof (Build))
            {
                var childObjects = GetChildObjects<TestAssemblyRepository, TestAssembly>(
                    TestAssemblyRepository.Instance, objectId);
                childObjects.ForEach(child => DeleteObjectsTreeRecurively<TestAssembly>(child.Id));

                var parentObjRepo = BuildRepository.Instance;
                parentObjRepo.Remove(parentObjRepo.Get(objectId));
            }
            else if (typeof (TEntityBase) == typeof (TestAssembly))
            {
                var childObjects = GetChildObjects<TestSuiteRepository, TestSuite>(TestSuiteRepository.Instance,
                    objectId);
                childObjects.ForEach(child => DeleteObjectsTreeRecurively<TestSuite>(child.Id));

                var parentObjRepo = TestAssemblyRepository.Instance;
                parentObjRepo.Remove(parentObjRepo.Get(objectId));
            }
            else if (typeof (TEntityBase) == typeof (TestSuite))
            {
                var childObjects = GetChildObjects<TestCaseRepository, TestCase>(TestCaseRepository.Instance, objectId);
                childObjects.ForEach(child => DeleteObjectsTreeRecurively<TestCase>(child.Id));

                var parentObjRepo = TestSuiteRepository.Instance;
                parentObjRepo.Remove(parentObjRepo.Get(objectId));
            }
            else if (typeof (TEntityBase) == typeof (TestCase))
            {
                var screenShotRepo = ScreenShotRepository.Instance;
                var childObjects = GetChildObjects<ScreenShotRepository, ScreenShot>(screenShotRepo, objectId);
                screenShotRepo.Remove(childObjects);
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