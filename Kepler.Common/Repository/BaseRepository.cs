using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using Dapper;
using Kepler.Common.Error;
using Kepler.Common.Models.Common;

namespace Kepler.Common.Repository
{
    public abstract class BaseRepository<TEntity> : IRepository<TEntity, long> where TEntity : InfoObject
    {
        /* private static object _lock = new object();*/
        // TODO: we should use Mutex (or some another sync primitive) and Pass Project Id variable (because everything should be locked only inside Project scrope)

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
                    return db.Get<TEntity>(id);
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


        public virtual void Update(TEntity entity)
        {
            using (var db = CreateConnection())
            {
                db.Open();
                using (var tran = db.BeginTransaction())
                {
                    try
                    {
                        db.Update(entity, tran);
                        tran.Commit();
                    }
                    catch (Exception ex)
                    {
                        tran.Rollback();
                        ErrorMessageRepository.Instance.Insert(new ErrorMessage()
                        {
                            ExceptionMessage = $"DB error: {ex.Message}"
                        });
                    }
                }
            }
        }

        public virtual void Update(IEnumerable<TEntity> entities)
        {
            using (var db = CreateConnection())
            {
                db.Open();
                using (var tran = db.BeginTransaction())
                {
                    try
                    {
                        db.Update(entities, tran);
                        tran.Commit();
                    }
                    catch (Exception ex)
                    {
                        tran.Rollback();
                        ErrorMessageRepository.Instance.Insert(new ErrorMessage()
                        {
                            ExceptionMessage = $"DB error: {ex.Message}"
                        });
                    }
                }
            }
        }


        public virtual void Insert(TEntity entity)
        {
            if (entity != null)
                return;

            using (var db = CreateConnection())
            {
                db.Open();
                using (var tran = db.BeginTransaction())
                {
                    try
                    {
                        db.Insert(entity, tran);
                        tran.Commit();
                    }
                    catch (Exception ex)
                    {
                        tran.Rollback();
                        ErrorMessageRepository.Instance.Insert(new ErrorMessage()
                        {
                            ExceptionMessage = $"DB error: {ex.Message}"
                        });
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
                        db.Delete(entity, tran);
                        tran.Commit();
                    }
                    catch (Exception ex)
                    {
                        tran.Rollback();
                        ErrorMessageRepository.Instance.Insert(new ErrorMessage()
                        {
                            ExceptionMessage = $"DB error: {ex.Message}"
                        });
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
                        db.Delete(entities, tran);
                        tran.Commit();
                    }
                    catch (Exception ex)
                    {
                        tran.Rollback();
                        ErrorMessageRepository.Instance.Insert(new ErrorMessage()
                        {
                            ExceptionMessage = $"DB error: {ex.Message}"
                        });
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
                    ErrorMessageRepository.Instance.Insert(new ErrorMessage()
                    {
                        ExceptionMessage = $"DB error: {ex.Message}"
                    });
                }
            }

            return null;
        }

        public virtual IEnumerable<TEntity> Find(string name)
        {
            using (var db = CreateConnection())
            {
                db.Open();
                try
                {
                    return db.GetList<TEntity>(new {Name = name});
                }
                catch (Exception ex)
                {
                    ErrorMessageRepository.Instance.Insert(new ErrorMessage()
                    {
                        ExceptionMessage = $"DB error: {ex.Message}"
                    });
                }
            }

            return null;
        }

        public virtual IEnumerable<TEntity> Find(Func<TEntity, bool> filterCondition)
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
                    ErrorMessageRepository.Instance.Insert(new ErrorMessage()
                    {
                        ExceptionMessage = $"DB error: {ex.Message}"
                    });
                }
            }

            return null;
        }
    }
}