using System.Collections.Generic;
using Kepler.Common.Models.Common;

namespace Kepler.Common.Models
{
    public class TestSuite : BuildObject
    {
        public Dictionary<long, TestCase> TestCases { get; set; }

        public TestSuite()
        {
        }

        public TestSuite(string Name) : base(Name)
        {
            TestCases = new Dictionary<long, TestCase>();
        }
    }
}