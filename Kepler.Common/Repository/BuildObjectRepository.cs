using System.Collections.Generic;
using Kepler.Common.Models.Common;

namespace Kepler.Common.Repository
{
    public class BuildObjectRepository<TBuildObjEntity> : BaseRepository<TBuildObjEntity>
        where TBuildObjEntity : BuildObject
    {
        protected BuildObjectRepository()
        {
        }


        public virtual IEnumerable<TBuildObjEntity> FindByParentId(long parentObjId)
        {
            return DbSet.Where(x => x.ParentObjId == parentObjId);
        }

        public virtual IEnumerable<TBuildObjEntity> FindByBuildId(long buildId)
        {
            return DbSet.Where(item => item.BuildId == buildId);
        }

        public virtual IEnumerable<TBuildObjEntity> FindFailedInBuild(long buildId)
        {
            return DbSet.Where(item => item.BuildId == buildId && item.Status == ObjectStatus.Failed);
        }
    }
}