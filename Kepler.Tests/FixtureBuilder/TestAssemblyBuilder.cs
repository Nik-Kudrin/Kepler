using System;
using Kepler.Core;

namespace Kepler.Tests.FixtureBuilder
{
    public class TestAssemblyBuilder : TestAssembly, IFixtureBuilder<TestAssembly>
    {
        public TestAssembly BuildValid()
        {
            throw new NotImplementedException();
        }
    }
}