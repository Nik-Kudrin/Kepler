using Kepler.Core.Common;

namespace Kepler.Core
{
    public class InfoObject
    {
        protected long ID;
        protected string Name;
        protected ObjectStatus Status;

        public InfoObject()
        {
            Status = ObjectStatus.Undefined;
        }
    }
}