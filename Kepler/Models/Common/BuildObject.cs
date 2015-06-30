namespace Kepler.Core.Common
{
    public class BuildObject : InfoObject
    {
        public long? BuildId { get; set; }

        public BuildObject()
        {
        }

        public BuildObject(string Name) : base(Name)
        {
        }
    }
}