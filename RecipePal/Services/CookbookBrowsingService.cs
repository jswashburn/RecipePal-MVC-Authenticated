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

        // TODO: EditCookbookViewModel
        // TODO: EditCookbooksViewModel
    }

    public class CookbookBrowsingService : ICookbookBrowsingService
    {
        readonly IRepository<Cookbook> _cookbooksRepo;
        readonly IRepository<Profile> _profilesRepo;
        readonly IRepository<Recipe> _recipesRepo;

        public CookbookBrowsingService(IRepository<Cookbook> cookbooks,
            IRepository<Profile> profiles, IRepository<Recipe> recipes)
        {
            _cookbooksRepo = cookbooks;
            _profilesRepo = profiles;
            _recipesRepo = recipes;
        }

        public AllCookbooksViewModel CreateAllCookbooksViewModel()
        {
            IEnumerable<Cookbook> allCookbooks = _cookbooksRepo.Get();
            foreach (Cookbook cookbook in allCookbooks)
            {
                cookbook.Recipes = GetRecipes(cookbook.Id);
                cookbook.OwnerProfile = GetProfile(cookbook.OwnerProfileId);
            }

            return new AllCookbooksViewModel { Cookbooks = allCookbooks };
        }

        public CookbookViewModel CreateCookbookViewModel(int cookbookId)
        {
            Cookbook cookbook = _cookbooksRepo.Get(cookbookId);
            cookbook.OwnerProfile = GetProfile(cookbook.OwnerProfileId);
            cookbook.Recipes = GetRecipes(cookbookId);

            return new CookbookViewModel { Cookbook = cookbook };
        }

        Profile GetProfile(int profileId) => 
            _profilesRepo.Get().FirstOrDefault(p => p.Id == profileId);

        List<Recipe> GetRecipes(int cookbookId) =>
            _recipesRepo.Get()
                .Where(r => r.CookbookId == cookbookId)
                .ToList();
    }
}
