using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Kepler.Common.DB;
using Kepler.Common.Models.Common;

namespace Kepler.Common.Repository
{
    public class BuildObjectRepository<TBuildObjEntity> : BaseRepository<TBuildObjEntity> where TBuildObjEntity : BuildObject
    {
        public virtual IEnumerable<TBuildObjEntity> Find(long parentObjId)
        {
            return _dbSet.Where(x => x.ParentObjId == parentObjId);
        }

        protected BuildObjectRepository(KeplerDataContext dbContext, DbSet<TBuildObjEntity> dbSet) : base(dbContext, dbSet)
        {
        }
    }
}