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
        readonly IRepository<Chef> _chefsRepo;
        readonly IRepository<Recipe> _recipesRepo;

        public CookbookBrowsingService(IRepository<Cookbook> cookbooks, IRepository<Chef> chefs,
            IRepository<Recipe> recipes)
        {
            _cookbooksRepo = cookbooks;
            _chefsRepo = chefs;
            _recipesRepo = recipes;
        }

        public AllCookbooksViewModel CreateAllCookbooksViewModel()
        {
            IEnumerable<Cookbook> allCookbooks = _cookbooksRepo.Get();
            foreach (Cookbook cookbook in allCookbooks)
            {
                cookbook.Recipes = GetRecipes(cookbook.Id);
                cookbook.Chef = GetChef(cookbook.ChefId);
            }

            var vm = new AllCookbooksViewModel { Cookbooks = allCookbooks };

            return vm;
        }

        public CookbookViewModel CreateCookbookViewModel(int cookbookId)
        {
            Cookbook cookbook = _cookbooksRepo.Get(cookbookId);
            cookbook.Chef = GetChef(cookbookId);
            cookbook.Recipes = GetRecipes(cookbookId);

            var vm = new CookbookViewModel { Cookbook = cookbook };

            return vm;
        }

        Chef GetChef(int id)
        {
            return _chefsRepo.Get(id);
        }

        List<Recipe> GetRecipes(int cookbookId)
        {
            return _recipesRepo.Get()
                .Where(r => r.CookbookId == cookbookId)
                .ToList();
        }
    }
}
