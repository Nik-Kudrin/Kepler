using System.Collections.Generic;
using System.Runtime.Serialization;
using Kepler.Core;

namespace Kepler.Models
{
    public class Project : InfoObject
    {
        public BaseLine BaseLine { get; set; }
        public Dictionary<long?, Build> Builds { get; set; }

        public Project()
        {
            Builds = new Dictionary<long?, Build>();
        }
    }
}