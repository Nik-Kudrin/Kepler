using System;
using System.Collections.Generic;
using Kepler.Core;

namespace Kepler.Tests.FixtureBuilder
{
    public class TestCaseBuilder : TestCase, IFixtureBuilder<TestCase>
    {
        public TestCase Build()
        {
            Id = BaseBuilder.GetRandomId();

            return this;
        }

        public TestCase BuildValid()
        {
            Build();

            Name = Guid.NewGuid().ToString();
            ScreenShots = new Dictionary<long?, ScreenShot>();


            return this;
        }


        public TestCaseBuilder AddTestSuite()
        {
            var testSuite = new TestSuiteBuilder().BuildValid();


            return this;
        }
    }
}