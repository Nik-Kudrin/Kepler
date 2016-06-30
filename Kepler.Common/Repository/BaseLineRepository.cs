using Kepler.Common.Models;

namespace Kepler.Common.Repository
{
    public class BaseLineRepository : BaseObjRepository<BaseLine>
    {
        public static BaseLineRepository Instance => new BaseLineRepository();

        private BaseLineRepository()
        {
        }
    }
}