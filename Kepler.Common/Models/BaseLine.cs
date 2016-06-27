using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using Kepler.Common.Models.Common;

namespace Kepler.Common.Models
{
    public class BaseLine : InfoObject
    {
        [DataMember]
        [Editable(true)]
        public long BranchId { get; set; }

        public Dictionary<long?, ScreenShot> ScreenShots { get; set; }
    }
}