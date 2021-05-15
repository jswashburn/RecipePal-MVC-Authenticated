using Microsoft.AspNetCore.Mvc;
using RecipePal.Services;
using RecipePal.ViewModels;

namespace RecipePal.Controllers
{
    public class RecipeController : Controller
    {
        readonly IRecipeBrowsingService _recipeBrowsingService;

        public RecipeController(IRecipeBrowsingService recipeBrowsingService)
        {
            _recipeBrowsingService = recipeBrowsingService;
        }

        public IActionResult Index(int id)
        {
            RecipeViewModel vm = _recipeBrowsingService.CreateRecipeViewModel(id);
            return View(vm);
        }
    }
}
