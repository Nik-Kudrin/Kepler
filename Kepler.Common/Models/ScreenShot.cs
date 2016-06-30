using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using Kepler.Common.Models.Common;

namespace Kepler.Common.Models
{
    public class ScreenShot : BuildObject
    {
        [DataMember]
        [StringLength(700)]
        [Dapper.Editable(true)]
        public string ImagePath { get; set; }

        [DataMember]
        [StringLength(700)]
        [Dapper.Editable(true)]
        public string ImagePathUrl { get; set; }

        [DataMember]
        [StringLength(700)]
        [Dapper.Editable(true)]
        public string PreviewImagePath { get; set; }

        [DataMember]
        [StringLength(700)]
        [Dapper.Editable(true)]
        public string PreviewImagePathUrl { get; set; }

        [DataMember]
        [StringLength(1000)]
        [Dapper.Editable(true)]
        public string DiffImagePath { get; set; }

        [DataMember]
        [StringLength(1000)]
        [Dapper.Editable(true)]
        public string DiffImagePathUrl { get; set; }

        [DataMember]
        [StringLength(1000)]
        [Dapper.Editable(true)]
        public string DiffPreviewPath { get; set; }

        [DataMember]
        [StringLength(1000)]
        [Dapper.Editable(true)]
        public string DiffPreviewPathUrl { get; set; }

        [DataMember]
        [StringLength(700)]
        public string BaseLineImagePath { get; set; }

        [DataMember]
        [StringLength(700)]
        [Dapper.Editable(true)]
        public string BaseLineImagePathUrl { get; set; }

        [DataMember]
        [StringLength(700)]
        [Dapper.Editable(true)]
        public string BaseLinePreviewPath { get; set; }

        [DataMember]
        [StringLength(700)]
        [Dapper.Editable(true)]
        public string BaseLinePreviewPathUrl { get; set; }

        [DataMember]
        [StringLength(500)]
        [Dapper.Editable(true)]
        public string Url { get; set; }

        [DataMember]
        [IgnoreDataMember]
        [Dapper.Editable(true)]
        public long BaseLineId { get; set; }

        [DataMember]
        [IgnoreDataMember]
        [Dapper.Editable(true)]
        public bool IsLastPassed { get; set; }

        [DataMember]
        [Dapper.Editable(true)]
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