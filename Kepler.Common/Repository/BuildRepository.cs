using System.Collections.Generic;
using Kepler.Common.DB;
using Kepler.Common.Models;
using Kepler.Common.Models.Common;

namespace Kepler.Common.Repository
{
    public class BuildRepository : BaseRepository<Build>
    {
        private static BuildRepository _repoInstance;

        public static BuildRepository Instance
        {
            get
            {
                if (_repoInstance == null)
                {
                    var dbContext = new KeplerDataContext();
                    _repoInstance = new BuildRepository(dbContext);
                }

                return _repoInstance;
            }
        }


        private BuildRepository(KeplerDataContext dbContext) : base(dbContext, dbContext.Builds)
        {
        }

        public IEnumerable<Build> GetInQueueBuilds()
        {
            return Find(build => build.Status == ObjectStatus.InQueue);
        }
    }
}