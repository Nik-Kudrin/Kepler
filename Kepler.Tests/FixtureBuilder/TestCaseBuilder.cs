using Kepler.Core;

namespace Kepler.Tests.FixtureBuilder
{
    public class TestCaseBuilder : TestCase, IFixtureBuilder<TestCase>
    {
        public TestCase BuildValid()
        {
            throw new System.NotImplementedException();
        }
    }
}