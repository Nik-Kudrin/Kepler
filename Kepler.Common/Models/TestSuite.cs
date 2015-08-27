using System.Collections.Generic;
using System.Runtime.Serialization;
using Kepler.Core.Common;

namespace Kepler.Core
{
    public class TestSuite : BuildObject
    {
        [DataMember]
        public Dictionary<long?, TestCase> TestCases { get; set; }

        public TestSuite()
        {
        }

        public TestSuite(string Name) : base(Name)
        {
            TestCases = new Dictionary<long?, TestCase>();
        }
    }
}