using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Kepler.Common.DB;
using Kepler.Common.Models.Common;

namespace Kepler.Common.Repository
{
    public class BuildObjectRepository<TBuildObjEntity> : BaseRepository<TBuildObjEntity>
        where TBuildObjEntity : BuildObject
    {
        public TBuildObjEntity GetCompleteObject(long id)
        {
            var entity = Get(id);
            (entity as IChildInit).InitChildObjectsFromDb();

            return entity;
        }

        public virtual IEnumerable<TBuildObjEntity> Find(long parentObjId)
        {
            return DbSet.Where(x => x.ParentObjId == parentObjId);
        }

        protected BuildObjectRepository(KeplerDataContext dbContext, DbSet<TBuildObjEntity> dbSet)
            : base(dbContext, dbSet)
        {
        }

        public IEnumerable<TBuildObjEntity> GetObjectsTreeByParentId(long parentObjId)
        {
            var items = Find(parentObjId).ToList();
            items.ForEach(item => (item as IChildInit).InitChildObjectsFromDb());

            return items;
        }
    }
}