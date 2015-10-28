using Kepler.Common.DB;
using Kepler.Common.Models;
using Kepler.Common.Models.Common;

namespace Kepler.Common.Repository
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

        public Branch GetCompleteObject(long id)
        {
            var entity = Get(id);
            (entity as IChildInit).InitChildObjectsFromDb();

            return entity;
        }
    }
}