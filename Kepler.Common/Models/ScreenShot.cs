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
        public string PreviewImagePath { get; set; }

        [DataMember]
        [StringLength(500)]
        public string DiffImagePath { get; set; }

        [DataMember]
        [StringLength(500)]
        public string DiffPreviewPath { get; set; }

        [DataMember]
        [StringLength(500)]
        public string BaseLineImagePath { get; set; }

        [DataMember]
        [StringLength(500)]
        public string BaseLinePreviewPath { get; set; }

        [DataMember]
        [StringLength(500)]
        public string Url { get; set; }

        [StringLength(700)]
        [DataMember]
        public string OriginalName { get; set; }

        [DataMember]
        [IgnoreDataMember]
        public long BaseLineId { get; set; }

        [DataMember]
        [IgnoreDataMember]
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