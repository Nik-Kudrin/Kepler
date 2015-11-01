using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Kepler.Common.DB;
using Kepler.Common.Error;

namespace Kepler.Common.Repository
{
    public class ErrorMessageRepository<TEntity> : IRepository<TEntity, long> where TEntity : ErrorMessage
    {
        protected readonly KeplerDataContext _dbContext;
        protected readonly DbSet<TEntity> _dbSet;


        protected ErrorMessageRepository(KeplerDataContext dbContext, DbSet<TEntity> dbSet)
        {
            _dbContext = dbContext;
            _dbSet = dbSet;
        }

        public virtual TEntity Get(long id)
        {
            return _dbSet.FirstOrDefault(x => x.Id == id);
        }

        public void Add(TEntity entity)
        {
            if (entity != null)
                _dbSet.Add(entity);
        }

        public void Update(TEntity entity)
        {
            if (entity != null)
            {
                _dbSet.Attach(entity);
                _dbContext.Entry(entity).State = EntityState.Modified;
            }
        }

        public void Update(IEnumerable<TEntity> entities)
        {
            foreach (var entity in entities)
            {
                _dbSet.Attach(entity);
                _dbContext.Entry(entity).State = EntityState.Modified;
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
            _dbContext.SaveChanges();
        }

        public void Remove(TEntity entity)
        {
            if (_dbContext.Entry(entity).State == EntityState.Detached)
            {
                _dbSet.Attach(entity);
            }

            _dbSet.Remove(entity);
        }

        public virtual IEnumerable<TEntity> FindAll()
        {
            return _dbSet.ToList();
        }

        public virtual IEnumerable<TEntity> Find(string name)
        {
            throw new NotImplementedException();
        }

        public virtual IEnumerable<TEntity> Find(Func<TEntity, bool> filterCondition)
        {
            return _dbSet.Where(filterCondition).ToList();
        }
    }
}