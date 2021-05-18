using RecipePal.Models.Abstractions;
using System.Collections.Generic;

namespace RecipePal.Repositories
{
    public interface IRepository<TModel> where TModel : IEntity
    {
        IEnumerable<TModel> Get();
        TModel Get(int id);
        TModel Insert(TModel item);
        TModel Delete(int id);
        TModel Update(TModel item);
        void Save();
    }
}
