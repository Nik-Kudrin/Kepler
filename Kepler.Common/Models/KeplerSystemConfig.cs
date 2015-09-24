using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace Kepler.Common.Models
{
    public class KeplerSystemConfig
    {
        [Key]
        [DataMember]
        public long Id { get; set; }

        [StringLength(500)]
        [DataMember]
        public string Name { get; set; }

        [StringLength(500)]
        [DataMember]
        public string Value { get; set; }


        public KeplerSystemConfig(string name, string value)
        {
            Name = name;
            Value = value;
        }
    }
}