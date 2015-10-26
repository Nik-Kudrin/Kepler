using Kepler.Common.DB;
using Kepler.Common.Models;

namespace Kepler.Common.Repository
{
    public class ProjectRepository : BaseRepository<Project>
    {
        private static ProjectRepository _repoInstance;

        public static ProjectRepository Instance
        {
            get
            {
                if (_repoInstance == null)
                {
                    var dbContext = new KeplerDataContext();
                    _repoInstance = new ProjectRepository(dbContext);
                }

                return _repoInstance;
            }
        }


        private ProjectRepository(KeplerDataContext dbContext) : base(dbContext, dbContext.Projects)
        {
        }
    }
}