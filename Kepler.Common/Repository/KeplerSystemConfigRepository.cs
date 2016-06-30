using Kepler.Common.Models;

namespace Kepler.Common.Repository
{
    public class KeplerSystemConfigRepository : BaseRepository<KeplerSystemConfig>
    {
        public static KeplerSystemConfigRepository Instance => new KeplerSystemConfigRepository();

        protected KeplerSystemConfigRepository()
        {
        }
    }
}