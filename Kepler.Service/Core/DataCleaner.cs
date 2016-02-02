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
        private static void DeleteDirectory(string dirPath)
        {
            var diffPath = Path.Combine(UrlPathGenerator.DiffImagePath, dirPath);
            var previewPath = Path.Combine(UrlPathGenerator.PreviewImagePath, dirPath);

            var paths = new List<string>();
            paths.Add(diffPath);
            paths.Add(previewPath);

            foreach (var path in paths)
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
        }

        private static void DeleteFile(string fileToDelete)
        {
            try
            {
                File.Delete(fileToDelete);
            }
            catch (Exception ex)
            {
                ErrorMessageRepository.Instance.Insert(new ErrorMessage() {ExceptionMessage = ex.Message});
            }
        }


        public static void DeleteObjectsTreeRecursively<TEntityBase>(long objectId, bool deleteDirectory = false)
            where TEntityBase : InfoObject
        {
            if (typeof (TEntityBase) == typeof (Project))
            {
                var childObjects = BranchRepository.Instance.Find(branch => branch.ProjectId == objectId).ToList();
                childObjects.ForEach(child => DeleteObjectsTreeRecursively<Branch>(child.Id));

                var parentObjRepo = ProjectRepository.Instance;
                var parentObj = parentObjRepo.Get(objectId);

                if (deleteDirectory)
                    DeleteDirectory(parentObj.Name);

                parentObjRepo.Delete(parentObj);
            }
            else if (typeof (TEntityBase) == typeof (Branch))
            {
                var childObjects = GetChildObjects<BuildRepository, Build>(BuildRepository.Instance, objectId);
                childObjects.ForEach(child => DeleteObjectsTreeRecursively<Build>(child.Id));

                var parentObjRepo = BranchRepository.Instance;
                var branchForDelete = parentObjRepo.Get(objectId);
                var project = ProjectRepository.Instance.Get(branchForDelete.ProjectId.Value);

                var baselineRepo = BaseLineRepository.Instance;
                var baseline = baselineRepo.Get(branchForDelete.BaseLineId.Value);
                baselineRepo.Delete(baseline);

                if (deleteDirectory)
                    DeleteDirectory(Path.Combine(project.Name, branchForDelete.Name));

                parentObjRepo.Delete(branchForDelete);

                if (branchForDelete.IsMainBranch)
                    project.MainBranchId = null;

                ProjectRepository.Instance.UpdateAndFlashChanges(project);
            }
            else if (typeof (TEntityBase) == typeof (Build))
            {
                var childObjects = GetChildObjects<TestAssemblyRepository, TestAssembly>(
                    TestAssemblyRepository.Instance, objectId);
                childObjects.ForEach(child => DeleteObjectsTreeRecursively<TestAssembly>(child.Id));

                var parentObjRepo = BuildRepository.Instance;
                var buildForDelete = parentObjRepo.Get(objectId);
                var branch = BranchRepository.Instance.Get(buildForDelete.ParentObjId.Value);
                var project = ProjectRepository.Instance.Get(branch.ProjectId.Value);

                if (deleteDirectory)
                    DeleteDirectory(Path.Combine(project.Name, branch.Name, buildForDelete.Id.ToString()));

                parentObjRepo.Delete(buildForDelete);
            }
            else if (typeof (TEntityBase) == typeof (TestAssembly))
            {
                var childObjects = GetChildObjects<TestSuiteRepository, TestSuite>(TestSuiteRepository.Instance,
                    objectId);
                childObjects.ForEach(child => DeleteObjectsTreeRecursively<TestSuite>(child.Id));

                var parentObjRepo = TestAssemblyRepository.Instance;
                parentObjRepo.Delete(parentObjRepo.Get(objectId));
            }
            else if (typeof (TEntityBase) == typeof (TestSuite))
            {
                var childObjects = GetChildObjects<TestCaseRepository, TestCase>(TestCaseRepository.Instance, objectId);
                childObjects.ForEach(child => DeleteObjectsTreeRecursively<TestCase>(child.Id));

                var parentObjRepo = TestSuiteRepository.Instance;
                parentObjRepo.Delete(parentObjRepo.Get(objectId));
            }
            else if (typeof (TEntityBase) == typeof (TestCase))
            {
                var screenShotRepo = ScreenShotRepository.Instance;
                var childObjects = GetChildObjects<ScreenShotRepository, ScreenShot>(screenShotRepo, objectId);

                // Delete source screenshots
                childObjects.ForEach(screenShot =>
                {
                    DeleteFile(screenShot.ImagePath);
                    DeleteFile(screenShot.PreviewImagePath);
                });

                screenShotRepo.Delete(childObjects);

                var parentObjRepo = TestCaseRepository.Instance;
                parentObjRepo.Delete(parentObjRepo.Get(objectId));
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