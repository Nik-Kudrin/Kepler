using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using Kepler.Common.Models.Common;

namespace Kepler.Common.Models
{
    public class ImageWorker : InfoObject
    {
        [StringLength(600)]
        [DataMember]
        [Dapper.Editable(true)]
        public string WorkerServiceUrl { get; set; }

        [DataMember]
        [Dapper.Editable(true)]
        public StatusOfWorker WorkerStatus { get; set; }

        public enum StatusOfWorker
        {
            Available,
            Offline
        }

        public ImageWorker()
        {
            WorkerStatus = StatusOfWorker.Offline;
        }
    }
}