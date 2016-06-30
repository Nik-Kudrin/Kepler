using System;
using System.Collections.Generic;
using Dapper;
using Kepler.Common.Error;
using Kepler.Common.Models;

namespace Kepler.Common.Repository
{
    public class ImageWorkerRepository : BaseObjRepository<ImageWorker>
    {
        public static ImageWorkerRepository Instance => new ImageWorkerRepository();

        private ImageWorkerRepository()
        {
        }

        public override IEnumerable<ImageWorker> Find(string workerServiceUrl)
        {
            using (var db = CreateConnection())
            {
                db.Open();
                try
                {
                    return db.GetList<ImageWorker>(new {WorkerServiceUrl = workerServiceUrl});
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