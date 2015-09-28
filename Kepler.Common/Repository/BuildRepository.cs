using System.Collections.Generic;
using Kepler.Core.Common;
using Kepler.Models;

namespace Kepler.Core
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