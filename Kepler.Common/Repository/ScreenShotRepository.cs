using System.Collections.Generic;
using System.Linq;
using Kepler.Common.Models;
using Kepler.Common.Models.Common;

namespace Kepler.Common.Repository
{
    public class ScreenShotRepository : BuildObjectRepository<ScreenShot>
    {
        public static ScreenShotRepository Instance => new ScreenShotRepository();

        private ScreenShotRepository()
        {
        }

        public IEnumerable<ScreenShot> GetInQueueScreenShotsForBuild(long buildId)
        {
            return Find(new {BuildId = buildId, Status = ObjectStatus.InQueue});
        }

        public IEnumerable<ScreenShot> GetAllInQueueScreenShots()
        {
            return Find(new {Status = ObjectStatus.InQueue});
        }

        public IEnumerable<ScreenShot> GetBaselineScreenShots(long baselineId)
        {
            return Find(new {BaseLineId = baselineId, IsLastPassed = true});
        }

        public ScreenShot GetBaselineScreenShot(long baselineId, string screenShotName)
        {
            return Find(new {BaseLineId = baselineId, IsLastPassed = true, Name = screenShotName})
                .FirstOrDefault();
        }
    }
}