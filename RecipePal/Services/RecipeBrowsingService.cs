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
        readonly IRepository<Recipe> _recipesRepo;
        readonly IRepository<Note> _notesRepo;

        public RecipeBrowsingService(IRepository<Recipe> recipes, IRepository<Note> notes)
        {
            _recipesRepo = recipes;
            _notesRepo = notes;
        }

        public RecipeViewModel CreateRecipeViewModel(int recipeId)
        {
            Recipe recipe = _recipesRepo.Get(recipeId);
            recipe.Notes = GetNotes(recipeId);

            RecipeViewModel vm = new RecipeViewModel { Recipe = recipe };
            return vm;
        }

        List<Note> GetNotes(int recipeId)
        {
            return _notesRepo.Get()
                .Where(n => n.RecipeId == recipeId)
                .ToList();
        }
    }
}
