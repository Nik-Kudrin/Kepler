using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using Kepler.Core;

namespace Kepler.Common.Models
{
    [DataContract]
    public class ImageWorker : InfoObject
    {
        [StringLength(600)]
        [DataMember]
        public string WorkerServiceUrl { get; set; }
    }
}