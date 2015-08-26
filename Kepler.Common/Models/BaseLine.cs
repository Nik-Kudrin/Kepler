using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace Kepler.Core
{
    public class BaseLine
    {
        [Key]
        [DataMember]
        public long Id { get; set; }

        public Dictionary<long?, ScreenShot> ScreenShots { get; set; }
    }
}