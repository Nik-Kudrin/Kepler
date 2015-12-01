using System.Collections.Generic;
using Kepler.Common.DB;
using Kepler.Common.Models;
using Kepler.Common.Models.Common;

namespace Kepler.Common.Repository
{
    public class BuildRepository : BaseRepository<Build>
    {
        public static BuildRepository Instance => new BuildRepository(new KeplerDataContext());

        private BuildRepository(KeplerDataContext dbContext) : base(dbContext, dbContext.Builds)
        {
        }

        public IEnumerable<Build> GetBuildsByStatus(ObjectStatus status)
        {
            return Find(build => build.Status == status);
        }
    }
}