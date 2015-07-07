using System.Data.Entity;
using Kepler.Models;

namespace Kepler.Core
{
    public class ProjectRepository : BaseRepository<Project>
    {
        public ProjectRepository(DbSet<Project> dbSet) : base(dbSet)
        {
        }
    }
}