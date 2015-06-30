using System.ComponentModel.DataAnnotations;
using Kepler.Core.Common;

namespace Kepler.Core
{
    public class InfoObject
    {
        [Key]
        public long ID;
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