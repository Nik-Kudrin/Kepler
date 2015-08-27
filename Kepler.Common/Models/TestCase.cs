using System.Collections.Generic;
using System.Runtime.Serialization;
using Kepler.Core.Common;

namespace Kepler.Core
{
    public class TestCase : BuildObject
    {
        [DataMember]
        public Dictionary<long?, ScreenShot> ScreenShots { get; set; }

        public TestCase()
        {
        }

        public TestCase(string Name) : base(Name)
        {
            ScreenShots = new Dictionary<long?, ScreenShot>();
        }
    }
}