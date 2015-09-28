using System.Collections.Generic;
using Kepler.Common.Core;
using Kepler.Core.Common;
using Kepler.Models;

namespace Kepler.Core
{
    public class ScreenShotRepository : BuildObjectRepository<ScreenShot>
    {
        private static ScreenShotRepository _repoInstance;

        public static ScreenShotRepository Instance
        {
            get
            {
                if (_repoInstance == null)
                {
                    var dbContext = new KeplerDataContext();
                    _repoInstance = new ScreenShotRepository(dbContext);
                }

                return _repoInstance;
            }
        }

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
            return Find(item => item.BaseLineId == baselineId && item.Status == ObjectStatus.Passed);
        }
    }
}