using System.Collections.Generic;
using System.Runtime.Serialization;
using Kepler.Core;

namespace Kepler.Models
{
    public class Project : InfoObject
    {
        [DataMember]
        public BaseLine BaseLine { get; set; }

        [DataMember]
        public Dictionary<long?, Build> Builds { get; set; }

        public Project()
        {
            Builds = new Dictionary<long?, Build>();
        }
    }
}