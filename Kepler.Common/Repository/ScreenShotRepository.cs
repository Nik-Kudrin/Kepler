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
            return Find(screenShot => screenShot.BuildId == buildId &&
                                      screenShot.Status == ObjectStatus.InQueue);
        }

        public IEnumerable<ScreenShot> GetAllInQueueScreenShots()
        {
            return Find(screenShot => screenShot.Status == ObjectStatus.InQueue);
        }

        public IEnumerable<ScreenShot> GetBaselineScreenShots(long baselineId)
        {
            return Find(item => item.BaseLineId == baselineId && item.IsLastPassed);
        }

        public ScreenShot GetBaselineScreenShot(long baselineId, string screenShotName)
        {
            return Find(item => item.BaseLineId == baselineId &&
                                item.IsLastPassed && item.Name == screenShotName)
                .FirstOrDefault();
        }
    }
}