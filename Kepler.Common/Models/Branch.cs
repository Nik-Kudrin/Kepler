using System.Collections.Generic;
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
        public long? BaseLineId { get; set; }

        [DataMember]
        public long? LatestBuildId { get; set; }

        [DataMember]
        public long? ProjectId { get; set; }

        [DataMember]
        public bool IsMainBranch { get; set; }

        public Dictionary<long, Build> Builds { get; set; }

        public Branch()
        {
            Builds = new Dictionary<long, Build>();
        }

        public void InitChildObjectsFromDb()
        {
            Builds = BuildRepository.Instance.Find(build => build.BranchId == Id)
                .ToDictionary(item => item.Id, item => item);
        }
    }
}