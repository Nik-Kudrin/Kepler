using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Kepler.Common.DB;
using Kepler.Common.Models;
using Kepler.Common.Models.Common;

namespace Kepler.Common.Repository
{
    public class BuildObjectRepository<TBuildObjEntity> : BaseRepository<TBuildObjEntity>
        where TBuildObjEntity : BuildObject
    {
        protected BuildObjectRepository(KeplerDataContext dbContext, DbSet<TBuildObjEntity> dbSet)
            : base(dbContext, dbSet)
        {
        }

        public TBuildObjEntity GetCompleteObject(long id)
        {
            var entity = Get(id);
            /*(entity as IChildInit).InitChildObjectsFromDb();*/
           // entity.InitChildObjectsFromDb<TBuildObjEntity>(new RepositoriesContainer());

            return entity;
        }

        public IEnumerable<TBuildObjEntity> GetObjectsTreeByParentId(long parentObjId)
        {
            var items = Find(parentObjId).ToList();
          //  items.ForEach(item => item.InitChildObjectsFromDb<TBuildObjEntity>(new RepositoriesContainer()));

            return items;
        }

        public virtual IEnumerable<TBuildObjEntity> Find(long parentObjId)
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