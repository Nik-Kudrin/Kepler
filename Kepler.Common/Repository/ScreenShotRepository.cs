using System.Collections.Generic;
using Kepler.Common.DB;
using Kepler.Common.Models;
using Kepler.Common.Models.Common;

namespace Kepler.Common.Repository
{
    public class ScreenShotRepository : BuildObjectRepository<ScreenShot>
    {
        private static ScreenShotRepository _repoInstance;

        public static ScreenShotRepository Instance => new ScreenShotRepository(new KeplerDataContext());

        private ScreenShotRepository(KeplerDataContext dbContext) : base(dbContext, dbContext.ScreenShots)
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
    }
}