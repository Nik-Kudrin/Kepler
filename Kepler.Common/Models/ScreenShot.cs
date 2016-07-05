using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using Kepler.Common.Models.Common;

namespace Kepler.Common.Models
{
    public class ScreenShot : BuildObject
    {
        [DataMember]
        [Dapper.Editable(true)]
        public string ImagePath { get; set; }

        [DataMember]
        [Dapper.Editable(true)]
        public string ImagePathUrl { get; set; }

        [DataMember]
        [Dapper.Editable(true)]
        public string PreviewImagePath { get; set; }

        [DataMember]
        [Dapper.Editable(true)]
        public string PreviewImagePathUrl { get; set; }

        [DataMember]
        [Dapper.Editable(true)]
        public string DiffImagePath { get; set; }

        [DataMember]
        [Dapper.Editable(true)]
        public string DiffImagePathUrl { get; set; }

        [DataMember]
        [Dapper.Editable(true)]
        public string DiffPreviewPath { get; set; }

        [DataMember]
        [Dapper.Editable(true)]
        public string DiffPreviewPathUrl { get; set; }

        [DataMember]
        public string BaseLineImagePath { get; set; }

        [DataMember]
        [Dapper.Editable(true)]
        public string BaseLineImagePathUrl { get; set; }

        [DataMember]
        [Dapper.Editable(true)]
        public string BaseLinePreviewPath { get; set; }

        [DataMember]
        [Dapper.Editable(true)]
        public string BaseLinePreviewPathUrl { get; set; }

        [DataMember]
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