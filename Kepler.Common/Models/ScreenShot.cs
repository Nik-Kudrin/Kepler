using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using Kepler.Core.Common;

namespace Kepler.Core
{
    public class ScreenShot : BuildObject
    {
        [DataMember]
        [StringLength(500)]
        public string ImagePath { get; set; }

        [DataMember]
        public long BaseLineId { get; set; }

        public ScreenShot()
        {
        }

        public ScreenShot(string Name, string ImagePath) : base(Name)
        {
            this.ImagePath = ImagePath;
        }
    }
}