using System.Collections.Generic;

namespace Kepler.Core
{
    public class Project : InfoObject
    {
        public BaseLine BaseLine { get; set; }
        public Dictionary<long?, Build> Builds { get; set; }
    }
}