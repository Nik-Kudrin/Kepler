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
        [Dapper.Editable(true)]
        public DateTime? StartDate { get; set; }

        [DataMember]
        [DataType(DataType.DateTime)]
        [Dapper.Editable(true)]
        public DateTime? StopDate { get; set; }

        [DataMember]
        [DataType(DataType.Time)]
        [Dapper.Editable(true)]
        public TimeSpan? Duration { get; set; }

        [DataMember]
        [DataType(DataType.Time)]
        [Dapper.Editable(true)]
        public TimeSpan? PredictedDuration { get; set; }

        [DataMember]
        [Dapper.Editable(true)]
        public long? BranchId { get; set; }

        [DataMember]
        [Dapper.Editable(true)]
        public long NumberTestAssemblies { get; set; }

        [DataMember]
        [Dapper.Editable(true)]
        public long NumberTestSuites { get; set; }

        [DataMember]
        [Dapper.Editable(true)]
        public long NumberTestCase { get; set; }

        [DataMember]
        [Dapper.Editable(true)]
        public long NumberScreenshots { get; set; }

        [DataMember]
        [Dapper.Editable(true)]
        public long NumberFailedScreenshots { get; set; }
    }
}