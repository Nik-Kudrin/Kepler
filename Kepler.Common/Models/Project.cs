using System.Collections.Generic;
using System.Runtime.Serialization;
using Kepler.Common.Models;
using Kepler.Core;

namespace Kepler.Models
{
    public class Project : InfoObject
    {
        public Dictionary<long?, Branch> Branches { get; set; }

        [DataMember]
        public long? MainBranchId { get; set; }

        public Project()
        {
            Branches = new Dictionary<long?, Branch>();
        }
    }
}