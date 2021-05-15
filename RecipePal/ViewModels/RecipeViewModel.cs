using RecipePal.Models;

namespace RecipePal.ViewModels
{
    public class RecipeViewModel
    {
        public bool CanEdit { get; set; }
        public Recipe Recipe { get; set; }
    }
}
