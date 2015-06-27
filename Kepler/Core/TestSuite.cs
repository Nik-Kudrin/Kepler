using System.Collections.Generic;
using Kepler.Core.Common;

namespace Kepler.Core
{
    public class TestSuite : BuildObject
    {
        public Dictionary<long?, TestCase> TestCases { get; set; }
    }
}