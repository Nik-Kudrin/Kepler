using Kepler.Common.Repository;

namespace Kepler.Common.Models.Common
{
    public interface ICompleteObject<TObjEntity>
    {
        TObjEntity GetCompleteObject(RepositoriesContainer repoContainer, long id);
    }
}