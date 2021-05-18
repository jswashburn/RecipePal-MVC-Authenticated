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
        readonly AppIdentityDbContext _context;

        public RepositoryFactory(AppIdentityDbContext context)
        {
            _context = context;
        }

        public IRepository<TModel> CreateRepository<TModel>() where TModel : class, IEntity
        {
            return new Repository<TModel>(_context);
        }
    }
}
