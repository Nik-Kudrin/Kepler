using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Kepler.Models;

namespace Kepler.Core
{
    public class BaseRepository<TEntity> : IRepository<TEntity, long> where TEntity : InfoObject
    {
        private readonly KeplerDataContext _dbContext;
        private readonly DbSet<TEntity> _dbSet;


        protected BaseRepository(KeplerDataContext dbContext, DbSet<TEntity> dbSet)
        {
            _dbContext = dbContext;
            _dbSet = dbSet;
        }

        public virtual TEntity Get(long id)
        {
            return _dbSet.Where(x => x.Id == id).FirstOrDefault();
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