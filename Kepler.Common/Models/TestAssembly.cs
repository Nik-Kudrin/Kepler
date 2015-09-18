using System.Collections.Generic;
using Kepler.Core.Common;

namespace Kepler.Core
{
    public class TestAssembly : BuildObject
    {
        public Dictionary<long, TestSuite> TestSuites { get; set; }

        public TestAssembly()
        {
        }

        public TestAssembly(string Name) : base(Name)
        {
            TestSuites = new Dictionary<long, TestSuite>();
        }
    }
}