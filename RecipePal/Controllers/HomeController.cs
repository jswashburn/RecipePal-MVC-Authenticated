using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RecipePal.Models;
using RecipePal.Services;
using RecipePal.ViewModels;
using System.Diagnostics;

namespace RecipePal.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        readonly ICookbookBrowsingService _cookbookBrowsingService;

        public HomeController(ICookbookBrowsingService cookbookBrowsingService)
        {
            _cookbookBrowsingService = cookbookBrowsingService;
        }

        [AllowAnonymous]
        public IActionResult Index()
        {
            AllCookbooksViewModel vm = _cookbookBrowsingService.CreateAllCookbooksViewModel();
            return View(vm);
        }

        [AllowAnonymous]
        public IActionResult Privacy()
        {
            return View();
        }

        [AllowAnonymous]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
