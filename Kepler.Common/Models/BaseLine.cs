using System.Collections.Generic;
using System.Runtime.Serialization;
using Kepler.Common.Models.Common;

namespace Kepler.Common.Models
{
    public class BaseLine : InfoObject
    {
        [DataMember]
        public long BranchId { get; set; }

        public Dictionary<long?, ScreenShot> ScreenShots { get; set; }
    }
}