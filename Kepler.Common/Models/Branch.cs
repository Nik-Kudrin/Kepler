using System.Collections.Generic;
using System.Runtime.Serialization;
using Kepler.Core;

namespace Kepler.Common.Models
{
    public class Branch : InfoObject
    {
        [DataMember]
        public long? BaseLineId { get; set; }

        [DataMember]
        public long? LatestBuildId { get; set; }

        [DataMember]
        public long? ProjectId { get; set; }

        [DataMember]
        public bool IsMainBranch { get; set; }

        public Dictionary<long?, Build> Builds { get; set; }

        public Branch()
        {
            Builds = new Dictionary<long?, Build>();
        }
    }
}