using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Dapper;
using Kepler.Common.Error;
using Kepler.Common.Models.Common;

namespace Kepler.Common.Repository
{
    public abstract class BaseRepository<TEntity> : IRepository<TEntity, long> where TEntity : InfoObject
    {
        /*private IDbConnection db;*/
        private string TableName;
        private static object _lock = new object();

        protected BaseRepository()
        {
            TableName = typeof (TEntity).Name;
        }

        protected SqlConnection CreateConnection()
        {
            return new SqlConnection(ConfigurationManager.ConnectionStrings["Kepler"].ConnectionString);
        }

        public virtual TEntity Get(long id)
        {
            lock (_lock)
            {
                using (var db = CreateConnection())
                {
                    try
                    {
                        var query = @"SELECT * FROM @TableName WHERE Id = @Id";
                        return db.Query<TEntity>(query, new {TableName = TableName, Id = id}).FirstOrDefault();
                    }
                    catch (Exception ex)
                    {
                        ErrorMessageRepository.Instance.Insert(new ErrorMessage()
                        {
                            ExceptionMessage = $"DB error: {ex.Message}"
                        });
                        return null;
                    }
                }
            }
        }

        public virtual void Add(TEntity entity)
        {
            if (entity != null)
                return;

            lock (_lock)
            {
                using (var db = CreateConnection())
                {
                    db.Open();
                    using (var tran = db.BeginTransaction())
                    {
                        try
                        {
                            var query = @"INSERT INTO @TableName ";
                            DbSet.Add(entity);

                            tran.Commit();
                        }
                        catch
                        {
                            tran.Rollback();
                            throw;
                        }
                    }
                }
            }
        }

        public virtual void Update(TEntity entity)
        {
            lock (_lock)
            {
                using (var db = CreateConnection())
                {
                }
            }


            if (entity != null)
            {
                DbSet.Attach(entity);
                DbContext.Entry(entity).State = EntityState.Modified;
            }
        }

        public virtual void Update(IEnumerable<TEntity> entities)
        {
            lock (_lock)
            {
                using (var db = CreateConnection())
                {
                }
            }


            entities.ToList().ForEach(Update);
        }

        public virtual void UpdateAndFlashChanges(TEntity entity)
        {
            lock (_lock)
            {
                using (var db = CreateConnection())
                {
                }
            }

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
            lock (_lock)
            {
                using (var db = CreateConnection())
                {
                }
            }


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
            lock (_lock)
            {
                using (var db = CreateConnection())
                {
                }
            }


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
            lock (_lock)
            {
                using (var db = CreateConnection())
                {
                }
            }


            DbContext.SaveChanges();
        }

        public virtual void Delete(TEntity entity)
        {
            lock (_lock)
            {
                using (var db = CreateConnection())
                {
                }
            }


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
            lock (_lock)
            {
                using (var db = CreateConnection())
                {
                }
            }


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
            lock (_lock)
            {
                using (var db = CreateConnection())
                {
                }
            }

            return DbSet.ToList();
        }

        public virtual IEnumerable<TEntity> Find(string name)
        {
            lock (_lock)
            {
                using (var db = CreateConnection())
                {
                }
            }


            return DbSet.Where(x => x.Name == name).ToList();
        }

        public virtual IEnumerable<TEntity> Find(Func<TEntity, bool> filterCondition)
        {
            lock (_lock)
            {
                using (var db = CreateConnection())
                {
                }
            }

            return DbSet.Where(filterCondition).ToList();
        }
    }
}