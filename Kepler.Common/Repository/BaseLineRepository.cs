using Kepler.Models;

namespace Kepler.Core
{
    public class BaseLineRepository : BaseRepository<BaseLine>
    {
        private static BaseLineRepository _repoInstance;

        public static BaseLineRepository Instance
        {
            get
            {
                if (_repoInstance == null)
                {
                    var dbContext = new KeplerDataContext();
                    _repoInstance = new BaseLineRepository(dbContext);
                }

                return _repoInstance;
            }
        }


        private BaseLineRepository(KeplerDataContext dbContext) : base(dbContext, dbContext.BaseLines)
        {
        }
    }
}