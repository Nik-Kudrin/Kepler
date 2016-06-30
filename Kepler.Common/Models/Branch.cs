using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using Kepler.Common.Models.Common;
using Kepler.Common.Repository;

namespace Kepler.Common.Models
{
    public class Branch : InfoObject, IChildInit
    {
        [DataMember]
        [IgnoreDataMember]
        [Dapper.Editable(true)]
        public long? BaseLineId { get; set; }

        [DataMember]
        [Dapper.Editable(true)]
        public long? LatestBuildId { get; set; }

        [DataMember]
        [Dapper.Editable(true)]
        public long? ProjectId { get; set; }

        [DataMember]
        [Dapper.Editable(true)]
        public bool IsMainBranch { get; set; }

        public Dictionary<long, Build> Builds { get; set; }

        public Branch()
        {
            Builds = new Dictionary<long, Build>();
        }


        public void InitChildObjectsFromDb<T, TEntityChild>(T childObjectRepository)
            where T : BaseObjRepository<TEntityChild> where TEntityChild : BuildObject
        {
            Builds = (childObjectRepository as BuildRepository)
                .Find(new {BranchId = Id})
                .ToDictionary(item => item.Id, item => item);
        }
    }
}