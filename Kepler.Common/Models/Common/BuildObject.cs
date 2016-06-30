using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using Kepler.Common.Repository;

namespace Kepler.Common.Models.Common
{
    public abstract class BuildObject : InfoObject
    {
        [DataMember]
        [Dapper.Editable(true)]
        public long? BuildId { get; set; }

        // For example: TestCase has Parent - TestSuite
        [DataMember]
        [Dapper.Editable(true)]
        public long? ParentObjId { get; set; }

        public BuildObject()
        {
        }

        public BuildObject(string Name) : base(Name)
        {
        }

        public Dictionary<long, TEntityChild> InitChildObjectsFromDb<T, TEntityChild>(T childObjectRepository)
            where T : BaseObjRepository<TEntityChild>
            where TEntityChild : BuildObject
        {
            return childObjectRepository.Find(new { ParentObjId = this.Id})
                .ToDictionary(item => item.Id, item => item);
        }
    }
}