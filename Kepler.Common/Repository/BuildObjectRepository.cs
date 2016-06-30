using System;
using System.Collections.Generic;
using Dapper;
using Kepler.Common.Error;
using Kepler.Common.Models.Common;

namespace Kepler.Common.Repository
{
    public class BuildObjectRepository<TBuildObjEntity> : BaseObjRepository<TBuildObjEntity>
        where TBuildObjEntity : BuildObject
    {
        protected BuildObjectRepository()
        {
        }


        public virtual IEnumerable<TBuildObjEntity> FindByParentId(long parentObjId)
        {
            using (var db = CreateConnection())
            {
                db.Open();
                try
                {
                    return db.GetList<TBuildObjEntity>(new {ParentObjId = parentObjId});
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

        public virtual IEnumerable<TBuildObjEntity> FindByBuildId(long buildId)
        {
            using (var db = CreateConnection())
            {
                db.Open();
                try
                {
                    return db.GetList<TBuildObjEntity>(new {BuildId = buildId});
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

        public virtual IEnumerable<TBuildObjEntity> FindFailedInBuild(long buildId)
        {
            using (var db = CreateConnection())
            {
                db.Open();
                try
                {
                    return db.GetList<TBuildObjEntity>(
                        new Func<TBuildObjEntity, bool>(
                            item => item.BuildId == buildId && item.Status == ObjectStatus.Failed));
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
}