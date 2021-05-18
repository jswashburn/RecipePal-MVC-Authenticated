using RecipePal.Models;
using RecipePal.Repositories;
using RecipePal.ViewModels;
using System.Collections.Generic;
using System.Linq;

namespace RecipePal.Services
{
    public interface IRecipeBrowsingService
    {
        RecipeViewModel CreateRecipeViewModel(int recipeId);
    }

    public class RecipeBrowsingService : IRecipeBrowsingService
    {
        readonly IRepositoryFactory _repositoryFactory;

        public RecipeBrowsingService(IRepositoryFactory repositoryFactory)
        {
            _repositoryFactory = repositoryFactory;
        }

        public RecipeViewModel CreateRecipeViewModel(int recipeId)
        {
            IRepository<Recipe> recipesRepo = _repositoryFactory.CreateRepository<Recipe>();

            Recipe recipe = recipesRepo.Get(recipeId);
            recipe.Notes = GetNotes(recipeId);

            RecipeViewModel vm = new RecipeViewModel { Recipe = recipe };
            return vm;
        }

        List<Note> GetNotes(int recipeId)
        {
            IRepository<Note> notesRepo = _repositoryFactory.CreateRepository<Note>();

            return notesRepo.Get()
                .Where(n => n.RecipeId == recipeId)
                .ToList();
        }
    }
}
