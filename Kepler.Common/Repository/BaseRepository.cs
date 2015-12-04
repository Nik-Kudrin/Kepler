using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Kepler.Common.DB;
using Kepler.Common.Error;
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
            try
            {
                return DbSet.FirstOrDefault(x => x.Id == id);
            }
            catch (Exception ex)
            {
                ErrorMessageRepository.Instance.Insert(new ErrorMessage() {ExceptionMessage = $"DB error: {ex.Message}"});
                return null;
            }
        }

        public virtual void Add(TEntity entity)
        {
            if (entity != null)
                DbSet.Add(entity);
        }

        public virtual void Update(TEntity entity)
        {
            if (entity != null)
            {
                DbSet.Attach(entity);
                DbContext.Entry(entity).State = EntityState.Modified;
            }
        }

        public virtual void Update(IEnumerable<TEntity> entities)
        {
            entities.ToList().ForEach(Update);
        }

        public virtual void UpdateAndFlashChanges(TEntity entity)
        {
            try
            {
                Update(entity);
                FlushChanges();
            }
            catch (Exception ex)
            {
                ErrorMessageRepository.Instance.Insert(new ErrorMessage() {ExceptionMessage = $"DB error: {ex.Message}"});
            }
        }

        public virtual void UpdateAndFlashChanges(IEnumerable<TEntity> entities)
        {
            try
            {
                Update(entities);
                FlushChanges();
            }
            catch (Exception ex)
            {
                ErrorMessageRepository.Instance.Insert(new ErrorMessage() {ExceptionMessage = $"DB error: {ex.Message}"});
            }
        }

        public virtual void Insert(TEntity entity)
        {
            try
            {
                Add(entity);
                FlushChanges();
            }
            catch (Exception ex)
            {
                ErrorMessageRepository.Instance.Insert(new ErrorMessage() {ExceptionMessage = $"DB error: {ex.Message}"});
            }
        }

        public virtual void FlushChanges()
        {
            DbContext.SaveChanges();
        }

        public virtual void Delete(TEntity entity)
        {
            try
            {
                if (DbContext.Entry(entity).State == EntityState.Detached)
                {
                    DbSet.Attach(entity);
                }

                DbSet.Remove(entity);
                FlushChanges();
            }
            catch (Exception ex)
            {
                ErrorMessageRepository.Instance.Insert(new ErrorMessage()
                {
                    ExceptionMessage = $"Trying to delete object: {ex.Message}"
                });
            }
        }

        public virtual void Delete(IEnumerable<TEntity> entities)
        {
            try
            {
                DbSet.RemoveRange(entities);
                FlushChanges();
            }
            catch (Exception ex)
            {
                ErrorMessageRepository.Instance.Insert(new ErrorMessage()
                {
                    ExceptionMessage = $"Trying to delete range of objects: {ex.Message}"
                });
            }
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