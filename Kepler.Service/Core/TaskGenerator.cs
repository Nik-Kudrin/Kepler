using System.Collections.Generic;
using Kepler.Core;
using Kepler.Core.Common;

namespace Kepler.Service.Core
{
    public class TaskGenerator
    {
        public IEnumerable<Build> GetInQueueBuilds()
        {
            return BuildRepository.Instance.Find(build => build.Status == ObjectStatus.InQueue);
        }

        public IEnumerable<ScreenShot> GetInQueueScreenShots(long buildId)
        {
            return ScreenShotRepository.Instance.Find(screenShot => screenShot.BuildId == buildId &&
                                                                    screenShot.Status == ObjectStatus.InQueue);
        }
    }
}