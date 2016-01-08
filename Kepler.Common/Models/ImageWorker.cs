using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using Kepler.Common.Models.Common;

namespace Kepler.Common.Models
{
    public class ImageWorker : InfoObject
    {
        [StringLength(600)]
        [DataMember]
        public string WorkerServiceUrl { get; set; }

        [DataMember]
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