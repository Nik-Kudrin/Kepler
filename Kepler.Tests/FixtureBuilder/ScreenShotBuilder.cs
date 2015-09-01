using System;
using Kepler.Core;
using Kepler.Core.Common;

namespace Kepler.Tests.FixtureBuilder
{
    public class ScreenShotBuilder : ScreenShot, IFixtureBuilder<ScreenShot>
    {
        public ScreenShot BuildValid()
        {
            Id = BaseBuilder.GetRandomId();
            ImagePath = Guid.NewGuid().ToString();
            Name = "Screenshot " + ImagePath;
            Status = ObjectStatus.Undefined;

            AddTestCase();

            return this;
        }

        public ScreenShotBuilder AddTestCase()
        {
            var testCase = new TestCaseBuilder().BuildValid();
            ParentObjId = testCase.Id;

            return this;
        }
    }
}