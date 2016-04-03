using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using Kepler.Common.Models.Common;

namespace Kepler.Common.Models
{
    public class BaseLine : InfoObject
    {
        [Index]
        [DataMember]
        public long BranchId { get; set; }

        public Dictionary<long?, ScreenShot> ScreenShots { get; set; }
    }
}