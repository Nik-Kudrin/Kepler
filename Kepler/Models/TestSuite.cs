using System.Collections.Generic;
using Kepler.Core.Common;

namespace Kepler.Core
{
    public class TestSuite : BuildObject
    {
        public Dictionary<long?, TestCase> TestCases { get; set; }

        public TestSuite()
        {
        }
/*
        public TestSuite(string Name) : base(Name)
        {
            TestCases = new Dictionary<long?, TestCase>();
        }*/
    }
}