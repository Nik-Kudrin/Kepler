using System.Collections.Generic;
using System.Linq;
using Kepler.Common.DB;
using Kepler.Common.Models;

namespace Kepler.Common.Repository
{
    public class ImageWorkerRepository : BaseRepository<ImageWorker>
    {
        private static ImageWorkerRepository _repoInstance;

        public static ImageWorkerRepository Instance => new ImageWorkerRepository(new KeplerDataContext());

        private ImageWorkerRepository(KeplerDataContext dbContext) : base(dbContext, dbContext.ImageWorkers)
        {
        }

        public override IEnumerable<ImageWorker> Find(string workerServiceUrl)
        {
            return DbSet.Where(worker => worker.WorkerServiceUrl == workerServiceUrl);
        }
    }
}