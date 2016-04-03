using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace Kepler.Common.Models.Common
{
    [DataContract]
    public abstract class InfoObject
    {
        [Index]
        [Key]
        [DataMember]
        public long Id { get; set; }

        [Index]
        [StringLength(700)]
        [DataMember]
        public string Name { get; set; }

        [Index]
        [DataMember]
        public ObjectStatus Status { get; set; }

        public InfoObject()
        {
            Status = ObjectStatus.Undefined;
        }

        public InfoObject(string Name) : this()
        {
            this.Name = Name;
        }
    }
}