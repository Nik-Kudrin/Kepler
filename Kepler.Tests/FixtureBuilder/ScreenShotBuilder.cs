using System;
using Kepler.Core;

namespace Kepler.Tests.FixtureBuilder
{
    public class ScreenShotBuilder : ScreenShot, IFixtureBuilder<ScreenShot>
    {
        public ScreenShot BuildValid()
        {
            throw new NotImplementedException();
        }
    }
}