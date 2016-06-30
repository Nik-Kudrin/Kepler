using Kepler.Common.Models.Common;

namespace Kepler.Common.Repository
{
    public abstract class BaseObjRepository<TEntity> : BaseRepository<TEntity> where TEntity : InfoObject
    {
    }
}