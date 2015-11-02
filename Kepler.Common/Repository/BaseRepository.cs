using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Kepler.Common.DB;
using Kepler.Common.Models.Common;

namespace Kepler.Common.Repository
{
    public class BaseRepository<TEntity> : IRepository<TEntity, long> where TEntity : InfoObject
    {
        protected readonly KeplerDataContext DbContext;
        protected readonly DbSet<TEntity> DbSet;


        protected BaseRepository(KeplerDataContext dbContext, DbSet<TEntity> dbSet)
        {
            DbContext = dbContext;
            DbSet = dbSet;
        }

        public virtual TEntity Get(long id)
        {
            return DbSet.FirstOrDefault(x => x.Id == id);
        }

        public void Add(TEntity entity)
        {
            if (entity != null)
                DbSet.Add(entity);
        }

        public void Update(TEntity entity)
        {
            if (entity != null)
            {
                DbSet.Attach(entity);
                DbContext.Entry(entity).State = EntityState.Modified;
            }
        }

        public void Update(IEnumerable<TEntity> entities)
        {
            foreach (var entity in entities)
            {
                DbSet.Attach(entity);
                DbContext.Entry(entity).State = EntityState.Modified;
            }
        }

        public void UpdateAndFlashChanges(TEntity entity)
        {
            Update(entity);
            FlushChanges();
        }

        public void UpdateAndFlashChanges(IEnumerable<TEntity> entities)
        {
            Update(entities);
            FlushChanges();
        }

        public void Insert(TEntity entity)
        {
            Add(entity);
            FlushChanges();
        }

        public virtual void FlushChanges()
        {
            DbContext.SaveChanges();
        }

        public void Remove(TEntity entity)
        {
            if (DbContext.Entry(entity).State == EntityState.Detached)
            {
                DbSet.Attach(entity);
            }

            DbSet.Remove(entity);
        }

        public virtual IEnumerable<TEntity> FindAll()
        {
            return DbSet.ToList();
        }

        public virtual IEnumerable<TEntity> Find(string name)
        {
            return DbSet.Where(x => x.Name == name).ToList();
        }

        public virtual IEnumerable<TEntity> Find(Func<TEntity, bool> filterCondition)
        {
            return DbSet.Where(filterCondition).ToList();
        }
    }
}