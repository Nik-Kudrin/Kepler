using System.Collections.Generic;
using Kepler.Core.Common;

namespace Kepler.Core
{
    public class TestCase : BuildObject
    {
        public Dictionary<long?, ScreenShot> ScreenShots { get; set; }
    }
}