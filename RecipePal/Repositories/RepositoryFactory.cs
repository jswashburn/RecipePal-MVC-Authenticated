using RecipePal.Data;
using RecipePal.Models.Abstractions;

namespace RecipePal.Repositories
{
    public interface IRepositoryFactory
    {
        IRepository<TModel> CreateRepository<TModel>() where TModel : class, IEntity;
    }

    public class RepositoryFactory : IRepositoryFactory
    {
        readonly RPDbContext _context;

        public RepositoryFactory(RPDbContext context)
        {
            _context = context;
        }

        public IRepository<TModel> CreateRepository<TModel>() where TModel : class, IEntity
        {
            return new Repository<TModel>(_context);
        }
    }
}
