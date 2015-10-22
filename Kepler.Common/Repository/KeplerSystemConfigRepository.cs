using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Kepler.Common.DB;
using Kepler.Common.Models;

namespace Kepler.Common.Repository
{
    public class KeplerSystemConfigRepository
    {
        protected readonly KeplerDataContext _dbContext;
        protected readonly DbSet<KeplerSystemConfig> _dbSet;

        private static KeplerSystemConfigRepository _repoInstance;

        public static KeplerSystemConfigRepository Instance
        {
            get
            {
                _repoInstance = _repoInstance ?? new KeplerSystemConfigRepository();
                return _repoInstance;
            }
        }


        protected KeplerSystemConfigRepository()
        {
            _dbContext = new KeplerDataContext();
            _dbSet = _dbContext.KeplerSystemConfig;
        }

        public KeplerSystemConfig Get(long id)
        {
            return _dbSet.FirstOrDefault(x => x.Id == id);
        }

        public void Add(KeplerSystemConfig entity)
        {
            _dbSet.Add(entity);
        }

        public void Update(KeplerSystemConfig entity)
        {
            _dbSet.Attach(entity);
            _dbContext.Entry(entity).State = EntityState.Modified;
        }

        public void Insert(KeplerSystemConfig entity)
        {
            Add(entity);
            FlushChanges();
        }

        public void FlushChanges()
        {
            _dbContext.SaveChanges();
        }

        public void Remove(KeplerSystemConfig entity)
        {
            if (_dbContext.Entry(entity).State == EntityState.Detached)
            {
                _dbSet.Attach(entity);
            }

            _dbSet.Remove(entity);
        }

        public IEnumerable<KeplerSystemConfig> FindAll()
        {
            return _dbSet.ToList();
        }

        public KeplerSystemConfig Find(string name)
        {
            return _dbSet.FirstOrDefault(x => x.Name == name);
        }

        public IEnumerable<KeplerSystemConfig> Find(Func<KeplerSystemConfig, bool> filterCondition)
        {
            return _dbSet.Where(filterCondition).ToList();
        }
    }
}