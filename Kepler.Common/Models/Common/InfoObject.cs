using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using Kepler.Core.Common;

namespace Kepler.Core
{
    [DataContract]
    public class InfoObject
    {
        [Key]
        public long Id { get; set; }

        [StringLength(500)]
        public string Name { get; set; }

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