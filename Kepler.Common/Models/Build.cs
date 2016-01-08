using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using Kepler.Common.Models.Common;

namespace Kepler.Common.Models
{
    public class Build : BuildObject
    {
        [DataMember]
        [DataType(DataType.DateTime)]
        public DateTime? StartDate { get; set; }

        [DataMember]
        [DataType(DataType.DateTime)]
        public DateTime? StopDate { get; set; }

        [DataMember]
        [DataType(DataType.Time)]
        public TimeSpan? Duration { get; set; }

        [DataMember]
        [DataType(DataType.Time)]
        public TimeSpan? PredictedDuration { get; set; }

        [DataMember]
        public long? BranchId { get; set; }

        [DataMember]
        public long NumberTestAssemblies { get; set; }

        [DataMember]
        public long NumberTestSuites { get; set; }

        [DataMember]
        public long NumberTestCase { get; set; }

        [DataMember]
        public long NumberScreenshots { get; set; }

        [DataMember]
        public long NumberFailedScreenshots { get; set; }
    }
}