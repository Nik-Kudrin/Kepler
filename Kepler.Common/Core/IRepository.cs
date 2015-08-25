using System.Collections.Generic;
using Kepler.Models;

namespace Kepler.Core
{
    public interface IRepository<TEntity, in TKey> where TEntity : class
    {
        TEntity Get(TKey id);
        void Save(TEntity entity);
        void Delete(TEntity entity);
        IEnumerable<TEntity> FindAll();
        IEnumerable<TEntity> Find(string name);
    }
}