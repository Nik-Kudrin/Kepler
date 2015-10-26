using System.Collections.Generic;
using Kepler.Common.Models.Common;

namespace Kepler.Common.Models
{
    public class TestCase : BuildObject
    {
        public Dictionary<long, ScreenShot> ScreenShots { get; set; }

        public TestCase()
        {
        }

        public TestCase(string Name) : base(Name)
        {
            ScreenShots = new Dictionary<long, ScreenShot>();
        }
    }
}