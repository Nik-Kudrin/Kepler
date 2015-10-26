using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using Kepler.Common.Models.Common;

namespace Kepler.Common.Models
{
    public class ScreenShot : BuildObject
    {
        [DataMember]
        [StringLength(500)]
        public string ImagePath { get; set; }

        [DataMember]
        [StringLength(500)]
        public string DiffImagePath { get; set; }

        [DataMember]
        public long BaseLineId { get; set; }

        [DataMember]
        public bool IsLastPassed { get; set; }

        [DataMember]
        public string ErrorMessage { get; set; }

        public ScreenShot()
        {
        }

        public ScreenShot(string Name, string ImagePath) : base(Name)
        {
            this.ImagePath = ImagePath;
        }
    }
}