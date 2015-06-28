namespace Kepler.Core.Common
{
    public class BuildObject : InfoObject
    {
        protected long? BuildId;

        public BuildObject()
        {
        }

        public BuildObject(string Name) : base(Name)
        {
        }
    }
}