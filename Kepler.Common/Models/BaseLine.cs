using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Kepler.Core
{
    public class BaseLine : InfoObject
    {
        [DataMember]
        public long BranchId { get; set; }

        public Dictionary<long?, ScreenShot> ScreenShots { get; set; }
    }
}