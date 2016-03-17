using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using Kepler.Common.Repository;

namespace Kepler.Common.Models.Common
{
    public abstract class BuildObject : InfoObject
    {
        [DataMember]
        public long? BuildId { get; set; }

        // For example: TestCase has Parent - TestSuite
        [DataMember]
        public long? ParentObjId { get; set; }

        public BuildObject()
        {
        }

        public BuildObject(string Name) : base(Name)
        {
        }

      /*  public Dictionary<long, TEntityChild> InitChildObjectsFromDb<TEntityChild>(RepositoriesContainer repoContainer)
            where TEntityChild : BuildObject
        {
            if (typeof (TEntityChild) == typeof (ScreenShot))
            {
                return repoContainer.ScreenShotRepo.Find(item => item.ParentObjId == this.Id)
                    .ToDictionary(item => item.Id, item => item as TEntityChild);
            }
            else if (typeof (TEntityChild) == typeof (TestCase))
            {
                return repoContainer.CaseRepo.Find(item => item.ParentObjId == this.Id)
                    .ToDictionary(item => item.Id, item => item as TEntityChild);
            }
            else if (typeof (TEntityChild) == typeof (TestSuite))
            {
                return repoContainer.SuiteRepo.Find(item => item.ParentObjId == this.Id)
                    .ToDictionary(item => item.Id, item => item as TEntityChild);
            }
            else if (typeof (TEntityChild) == typeof (TestAssembly))
            {
                return repoContainer.AssemblyRepo.Find(item => item.ParentObjId == this.Id)
                    .ToDictionary(item => item.Id, item => item as TEntityChild);
            }

            return null;
        }*/

        public Dictionary<long, TEntityChild> InitChildObjectsFromDb<T, TEntityChild>(T childObjectRepository)
            where T : BaseRepository<TEntityChild>
            where TEntityChild : BuildObject
        {
            return childObjectRepository.Find(item => item.ParentObjId == this.Id)
                .ToDictionary(item => item.Id, item => item);
        }
    }
}