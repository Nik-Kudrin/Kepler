using Kepler.Common.Models;
using Kepler.Models;

namespace Kepler.Core
{
    public class BranchRepository : BaseRepository<Branch>
    {
        private static BranchRepository _repoInstance;

        public static BranchRepository Instance
        {
            get
            {
                if (_repoInstance == null)
                {
                    var dbContext = new KeplerDataContext();
                    _repoInstance = new BranchRepository(dbContext);
                }

                return _repoInstance;
            }
        }


        private BranchRepository(KeplerDataContext dbContext) : base(dbContext, dbContext.Branches)
        {
        }
    }
}