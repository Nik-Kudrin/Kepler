using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Kepler.Core;
using Kepler.Core.Common;
using Kepler.Models;

namespace Kepler.Common.Core
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