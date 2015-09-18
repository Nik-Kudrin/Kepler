using System.Runtime.Serialization;

namespace Kepler.Core.Common
{
    public class BuildObject : InfoObject
    {
        [DataMember]
        public long? BuildId { get; set; }

        // For example: TestCase has Parent - TestSuite
        [DataMember]
        public  long? ParentObjId { get; set; }

        public BuildObject()
        {
        }

        public BuildObject(string Name) : base(Name)
        {
        }
    }
}