using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using Kepler.Core.Common;

namespace Kepler.Core
{
    public class BaseLine
    {
        [Key]
        [DataMember]
        public long Id { get; set; }

        [DataMember]
        public long ProjectId { get; set; }

        [DataMember]
        public ObjectStatus Status { get; set; }

        [DataMember]
        public Dictionary<long?, ScreenShot> ScreenShots { get; set; }
    }
}