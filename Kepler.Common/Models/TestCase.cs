using System.Collections.Generic;
using Kepler.Common.Models.Common;
using Kepler.Common.Repository;

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

        public void InitChildObjectsFromDb(RepositoriesContainer repoContainer)
        {
            ScreenShots = base.InitChildObjectsFromDb<ScreenShotRepository, ScreenShot>(repoContainer.ScreenShotRepo);
        }
    }
}