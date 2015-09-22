using Kepler.Common.Core;
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
    }
}