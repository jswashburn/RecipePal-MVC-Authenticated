using RecipePal.Models;

namespace RecipePal.ViewModels
{
    public class CookbookViewModel
    {
        public bool CanEdit { get; set; }
        public Cookbook Cookbook { get; set; }
    }
}
