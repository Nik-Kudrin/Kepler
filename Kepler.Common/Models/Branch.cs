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
        [Editable(true)]
        public long? BaseLineId { get; set; }

        [DataMember]
        [Editable(true)]
        public long? LatestBuildId { get; set; }

        [DataMember]
        [Editable(true)]
        public long? ProjectId { get; set; }

        [DataMember]
        [Editable(true)]
        public bool IsMainBranch { get; set; }

        public Dictionary<long, Build> Builds { get; set; }

        public Branch()
        {
            Builds = new Dictionary<long, Build>();
        }


        public void InitChildObjectsFromDb<T, TEntityChild>(T childObjectRepository)
            where T : BaseRepository<TEntityChild> where TEntityChild : BuildObject
        {
            Builds = (childObjectRepository as BuildRepository)
                .Find(build => build.BranchId == Id)
                .ToDictionary(item => item.Id, item => item);
        }
    }
}