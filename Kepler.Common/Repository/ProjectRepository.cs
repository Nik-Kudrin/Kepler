using Kepler.Common.DB;
using Kepler.Common.Models;
using Kepler.Common.Models.Common;

namespace Kepler.Common.Repository
{
    public class ProjectRepository : BaseRepository<Project>
    {
        public static ProjectRepository Instance => new ProjectRepository(new KeplerDataContext());

        private ProjectRepository(KeplerDataContext dbContext) : base(dbContext, dbContext.Projects)
        {
        }

        public Project GetCompleteObject(long id)
        {
            var entity = Get(id);
            entity.InitChildObjectsFromDb();

            return entity;
        }
    }
}