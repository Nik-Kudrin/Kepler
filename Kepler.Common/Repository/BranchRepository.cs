using Kepler.Common.Models;
using Kepler.Common.Models.Common;

namespace Kepler.Common.Repository
{
    public class BranchRepository : BaseRepository<Branch>
    {
        public static BranchRepository Instance => new BranchRepository();

        private BranchRepository()
        {
        }

        public Branch GetCompleteObject(long id)
        {
            var entity = Get(id);
            (entity as IChildInit).InitChildObjectsFromDb<BuildRepository, Build>(BuildRepository.Instance);

            return entity;
        }
    }
}