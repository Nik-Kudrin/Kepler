using Kepler.Common.Repository;

namespace Kepler.Common.Models.Common
{
    public interface IChildInit
    {
        void InitChildObjectsFromDb<T, TEntityChild>(T childObjectRepository)
            where T : BaseRepository<TEntityChild>
            where TEntityChild : BuildObject;
    }
}