﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Kepler.Common.DB;
using Kepler.Common.Error;

namespace Kepler.Common.Repository
{
    public class ErrorMessageRepository : IRepository<ErrorMessage, long>
    {
        private readonly KeplerDataContext _dbContext;
        private readonly DbSet<ErrorMessage> _dbSet;

        public static ErrorMessageRepository Instance
        {
            get
            {
                var dbContext = new KeplerDataContext();
                return new ErrorMessageRepository(dbContext, dbContext.ErrorMessages);
            }
        }

        protected ErrorMessageRepository(KeplerDataContext dbContext, DbSet<ErrorMessage> dbSet)
        {
            _dbContext = dbContext;
            _dbSet = dbSet;
        }

        public virtual ErrorMessage Get(long id)
        {
            return _dbSet.FirstOrDefault(x => x.Id == id);
        }

        public void Add(ErrorMessage entity)
        {
            if (entity != null)
                _dbSet.Add(entity);
        }

        public void Update(ErrorMessage entity)
        {
            if (entity != null)
            {
                _dbSet.Attach(entity);
                _dbContext.Entry(entity).State = EntityState.Modified;
            }
        }

        public void Update(IEnumerable<ErrorMessage> entities)
        {
            foreach (var entity in entities)
            {
                _dbSet.Attach(entity);
                _dbContext.Entry(entity).State = EntityState.Modified;
            }
        }

        public void UpdateAndFlashChanges(ErrorMessage entity)
        {
            Update(entity);
            FlushChanges();
        }

        public void UpdateAndFlashChanges(IEnumerable<ErrorMessage> entities)
        {
            Update(entities);
            FlushChanges();
        }

        public void Insert(ErrorMessage entity)
        {
            Add(entity);
            FlushChanges();
        }

        public virtual void FlushChanges()
        {
            _dbContext.SaveChanges();
        }

        public void Delete(ErrorMessage entity)
        {
            if (_dbContext.Entry(entity).State == EntityState.Detached)
            {
                _dbSet.Attach(entity);
            }

            _dbSet.Remove(entity);
            FlushChanges();
        }

        public virtual IEnumerable<ErrorMessage> FindAll()
        {
            return _dbSet.ToList();
        }

        public virtual IEnumerable<ErrorMessage> Find(string name)
        {
            throw new NotImplementedException();
        }

        public virtual IEnumerable<ErrorMessage> Find(Func<ErrorMessage, bool> filterCondition)
        {
            return _dbSet.Where(filterCondition).ToList();
        }
    }
}