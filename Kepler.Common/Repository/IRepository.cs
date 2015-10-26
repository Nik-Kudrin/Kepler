using System.Collections.Generic;

namespace Kepler.Common.Repository
{
    public interface IRepository<TEntity, in TKey> where TEntity : class
    {
        TEntity Get(TKey id);
        void Update(TEntity entity);
        void Remove(TEntity entity);
        IEnumerable<TEntity> FindAll();
        IEnumerable<TEntity> Find(string name);
    }
}