using Kepler.Core.Common;

namespace Kepler.Core
{
    public class InfoObject
    {
        protected long ID;
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