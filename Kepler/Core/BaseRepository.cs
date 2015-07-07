using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace Kepler.Core
{
    public class BaseRepository<TEntity> : IRepository<TEntity, long> where TEntity : InfoObject
    {
        private readonly DbSet<TEntity> _dbSet;

        public BaseRepository(DbSet<TEntity> dbSet)
        {
            _dbSet = dbSet;
        }

        public TEntity Get(long id)
        {
            return _dbSet.Where(x => x.Id == id).FirstOrDefault();
        }

        public void Save(TEntity entity)
        {
            _dbSet.Attach(entity);
        }

        public void Delete(TEntity entity)
        {
            _dbSet.Remove(entity);
        }

        public IEnumerable<TEntity> FindAll()
        {
            return _dbSet.ToList();
        }

        public IEnumerable<TEntity> Find(string name)
        {
            return _dbSet.Where(x => x.Name == name).ToList();
        }
    }
}