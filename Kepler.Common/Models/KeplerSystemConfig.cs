using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace Kepler.Common.Models
{
    public class KeplerSystemConfig
    {
        [Dapper.Key]
        [DataMember]
        public long Id { get; set; }

        [StringLength(500)]
        [DataMember]
        [Dapper.Editable(true)]
        public string Name { get; set; }

        [StringLength(500)]
        [DataMember]
        [Dapper.Editable(true)]
        public string Value { get; set; }

        public KeplerSystemConfig()
        {
        }

        public KeplerSystemConfig(string name, string value)
        {
            Name = name;
            Value = value;
        }
    }
}