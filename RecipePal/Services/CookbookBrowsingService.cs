using RecipePal.Models;
using RecipePal.Repositories;
using RecipePal.ViewModels;
using System.Collections.Generic;
using System.Linq;

namespace RecipePal.Services
{
    public interface ICookbookBrowsingService
    {
        AllCookbooksViewModel CreateAllCookbooksViewModel();
        CookbookViewModel CreateCookbookViewModel(int cookbookId);
    }

    public class CookbookBrowsingService : ICookbookBrowsingService
    {
        readonly IRepositoryFactory _repositoryFactory;

        public CookbookBrowsingService(IRepositoryFactory repositoryFactory)
        {
            _repositoryFactory = repositoryFactory;
        }

        public AllCookbooksViewModel CreateAllCookbooksViewModel()
        {
            IRepository<Cookbook> cookbooksRepo = _repositoryFactory.CreateRepository<Cookbook>();

            IEnumerable<Cookbook> allCookbooks = cookbooksRepo.Get();
            foreach (Cookbook cookbook in allCookbooks)
            {
                cookbook.Recipes = GetRecipes(cookbook.Id);
                cookbook.OwnerProfile = GetProfile(cookbook.OwnerProfileId);
            }

            return new AllCookbooksViewModel { Cookbooks = allCookbooks };
        }

        public CookbookViewModel CreateCookbookViewModel(int cookbookId)
        {
            IRepository<Cookbook> cookbooksRepo = _repositoryFactory.CreateRepository<Cookbook>();

            Cookbook cookbook = cookbooksRepo.Get(cookbookId);
            cookbook.OwnerProfile = GetProfile(cookbook.OwnerProfileId);
            cookbook.Recipes = GetRecipes(cookbookId);

            return new CookbookViewModel { Cookbook = cookbook };
        }

        Profile GetProfile(int profileId)
        {
            IRepository<Profile> profilesRepo = _repositoryFactory.CreateRepository<Profile>();

            return profilesRepo.Get().FirstOrDefault(p => p.Id == profileId);
        }

        List<Recipe> GetRecipes(int cookbookId)
        {
            IRepository<Recipe> recipesRepo = _repositoryFactory.CreateRepository<Recipe>();

            return recipesRepo.Get()
                .Where(r => r.CookbookId == cookbookId)
                .ToList();
        }
    }
}
