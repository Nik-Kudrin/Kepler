using Kepler.Common.DB;
using Kepler.Common.Models;

namespace Kepler.Common.Repository
{
    public class BaseLineRepository : BaseRepository<BaseLine>
    {
        public static BaseLineRepository Instance => new BaseLineRepository(new KeplerDataContext());

        private BaseLineRepository(KeplerDataContext dbContext) : base(dbContext.BaseLines)
        {
        }
    }
}