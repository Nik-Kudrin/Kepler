using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using Dapper;
using Kepler.Common.Error;

namespace Kepler.Common.Repository
{
    public abstract class BaseRepository<TEntity> : IRepository<TEntity, long> where TEntity : class
    {
        protected BaseRepository()
        {
        }

        protected SqlConnection CreateConnection()
        {
            return new SqlConnection(ConfigurationManager.ConnectionStrings["Kepler"].ConnectionString);
        }

        public virtual TEntity Get(long id)
        {
            using (var db = CreateConnection())
            {
                db.Open();
                try
                {
                    return SimpleCRUD.Get<TEntity>(db, id);
                }
                catch (Exception ex)
                {
                    LogErrorMessage(typeof (TEntity), ex);
                }
            }

            return null;
        }


        public virtual void Update(TEntity entity)
        {
            if (entity == null)
                return;

            using (var db = CreateConnection())
            {
                db.Open();
                using (var tran = db.BeginTransaction())
                {
                    try
                    {
                        SimpleCRUD.Update(db, entity, tran);
                        tran.Commit();
                    }
                    catch (Exception ex)
                    {
                        tran.Rollback();
                        LogErrorMessage(typeof (TEntity), ex);
                    }
                }
            }
        }

        public virtual void Update(IEnumerable<TEntity> entities)
        {
            if (entities == null)
                return;

            foreach (var entity in entities)
            {
                Update(entity);
            }
        }


        public virtual void Insert(TEntity entity)
        {
            if (entity == null)
                return;

            using (var db = CreateConnection())
            {
                db.Open();
                using (var tran = db.BeginTransaction())
                {
                    try
                    {
                        SimpleCRUD.Insert(db, entity, tran);
                        tran.Commit();
                    }
                    catch (Exception ex)
                    {
                        tran.Rollback();
                        LogErrorMessage(typeof (TEntity), ex);
                    }
                }
            }
        }

        public virtual void Delete(TEntity entity)
        {
            using (var db = CreateConnection())
            {
                db.Open();
                using (var tran = db.BeginTransaction())
                {
                    try
                    {
                        SimpleCRUD.Delete(db, entity, tran);
                        tran.Commit();
                    }
                    catch (Exception ex)
                    {
                        tran.Rollback();
                        LogErrorMessage(typeof (TEntity), ex);
                    }
                }
            }
        }

        public virtual void Delete(IEnumerable<TEntity> entities)
        {
            using (var db = CreateConnection())
            {
                db.Open();
                using (var tran = db.BeginTransaction())
                {
                    try
                    {
                        SimpleCRUD.Delete(db, entities, tran);
                        tran.Commit();
                    }
                    catch (Exception ex)
                    {
                        tran.Rollback();
                        LogErrorMessage(typeof (TEntity), ex);
                    }
                }
            }
        }

        public virtual IEnumerable<TEntity> FindAll()
        {
            using (var db = CreateConnection())
            {
                db.Open();
                try
                {
                    return db.GetList<TEntity>();
                }
                catch (Exception ex)
                {
                    LogErrorMessage(typeof (TEntity), ex);
                }
            }

            return new List<TEntity>();
        }

        public virtual IEnumerable<TEntity> Find(object filterCondition)
        {
            using (var db = CreateConnection())
            {
                db.Open();
                try
                {
                    return SimpleCRUD.GetList<TEntity>(db, filterCondition);
                }
                catch (Exception ex)
                {
                    LogErrorMessage(typeof (TEntity), ex);
                }
            }

            return new List<TEntity>();
        }

        public virtual IEnumerable<TEntity> Find(string filterCondition)
        {
            using (var db = CreateConnection())
            {
                db.Open();
                try
                {
                    return db.GetList<TEntity>(filterCondition);
                }
                catch (Exception ex)
                {
                    LogErrorMessage(typeof (TEntity), ex);
                }
            }

            return new List<TEntity>();
        }

        private void LogErrorMessage(Type entityType, Exception ex)
        {
            ErrorMessageRepository.Instance.Insert(new ErrorMessage()
            {
                ExceptionMessage = $"DB error: {entityType.FullName} {ex.Message} {ex.StackTrace}"
            });
        }
    }
}