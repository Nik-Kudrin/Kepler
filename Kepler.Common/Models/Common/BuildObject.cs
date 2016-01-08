using System.Collections.Generic;
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

        protected Dictionary<long, TEntityChild> InitChildObjectsFromDb<T, TEntityChild>(T childObjectRepository)
            where T : BaseRepository<TEntityChild>
            where TEntityChild : BuildObject
        {
            return childObjectRepository.Find(item => item.ParentObjId == this.Id)
                .ToDictionary(item => item.Id, item => item);
        }
    }
}