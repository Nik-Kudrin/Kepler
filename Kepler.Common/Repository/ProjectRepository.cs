using Kepler.Common.Models;

namespace Kepler.Common.Repository
{
    public class ProjectRepository : BaseRepository<Project>
    {
        public static ProjectRepository Instance => new ProjectRepository();

        private ProjectRepository()
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