using System.Collections.Generic;
using Kepler.Common.Models;

namespace Kepler.Common.Repository
{
    public class ImageWorkerRepository : BaseRepository<ImageWorker>
    {
        public static ImageWorkerRepository Instance => new ImageWorkerRepository();

        private ImageWorkerRepository()
        {
        }

        public override IEnumerable<ImageWorker> Find(string workerServiceUrl)
        {
            return DbSet.Where(worker => worker.WorkerServiceUrl == workerServiceUrl);
        }
    }
}