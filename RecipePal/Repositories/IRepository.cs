using RecipePal.Models.Abstractions;
using System.Collections.Generic;

namespace RecipePal.Repositories
{
    public interface IRepository<T> where T : IEntity
    {
        IEnumerable<T> Get();
        T Get(int id);
        T Insert(T item);
        T Delete(int id);
        T Update(T item);
        void Save();
    }
}
