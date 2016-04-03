using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;
using System.Linq;
using System.Runtime.Serialization;
using Kepler.Common.Repository;

namespace Kepler.Common.Models.Common
{
    public abstract class BuildObject : InfoObject
    {
        [Index]
        [DataMember]
        public long? BuildId { get; set; }

        // For example: TestCase has Parent - TestSuite
        [Index]
        [DataMember]
        public long? ParentObjId { get; set; }

        public BuildObject()
        {
        }

        public BuildObject(string Name) : base(Name)
        {
        }

        public Dictionary<long, TEntityChild> InitChildObjectsFromDb<T, TEntityChild>(T childObjectRepository)
            where T : BaseRepository<TEntityChild>
            where TEntityChild : BuildObject
        {
            return childObjectRepository.Find(item => item.ParentObjId == this.Id)
                .ToDictionary(item => item.Id, item => item);
        }
    }
}