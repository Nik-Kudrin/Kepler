using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Kepler.Core.Common;
using Kepler.Models;

namespace Kepler.Core
{
    public class BaseRepository<TEntity> : IRepository<TEntity, long> where TEntity : InfoObject
    {
        protected readonly KeplerDataContext _dbContext;
        protected readonly DbSet<TEntity> _dbSet;


        protected BaseRepository(KeplerDataContext dbContext, DbSet<TEntity> dbSet)
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
            _dbSet.Add(entity);
        }

        public void Save(TEntity entity)
        {
            _dbSet.Attach(entity);
        }

        public virtual void FlushChanges()
        {
            _dbContext.SaveChanges();
        }

        public void Delete(TEntity entity)
        {
            _dbSet.Remove(entity);
        }

        public virtual IEnumerable<TEntity> FindAll()
        {
            return _dbSet.ToList();
        }

        public virtual IEnumerable<TEntity> Find(string name)
        {
            return _dbSet.Where(x => x.Name == name).ToList();
        }
    }
}