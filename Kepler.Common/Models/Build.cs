using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace Kepler.Core
{
    public class Build : InfoObject
    {
        [DataMember]
        [DataType(DataType.DateTime)]
        public DateTime? StartDate { get; set; }

        [DataMember]
        [DataType(DataType.Time)]
        public TimeSpan? Duration { get; set; }

        [DataMember]
        public long TestCount { get; set; }

        [DataMember]
        public long TestsFailed { get; set; }
    }
}