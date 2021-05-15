using Microsoft.AspNetCore.Mvc;
using RecipePal.Services;
using RecipePal.ViewModels;

namespace RecipePal.Controllers
{
    public class CookbookController : Controller
    {
        readonly ICookbookBrowsingService _cookbookBrowsingService;

        public CookbookController(ICookbookBrowsingService cookbookBrowsingService)
        {
            _cookbookBrowsingService = cookbookBrowsingService;
        }

        public IActionResult Index(int id)
        {
            CookbookViewModel vm = _cookbookBrowsingService.CreateCookbookViewModel(id);
            vm.CanEdit = true;
            return View(vm);
        }
    }
}
