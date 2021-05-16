using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RecipePal.Models;
using RecipePal.Services;
using RecipePal.ViewModels;
using System.Diagnostics;

namespace RecipePal.Controllers
{
    public class HomeController : Controller
    {
        readonly ICookbookBrowsingService _cookbookBrowsingService;

        public HomeController(ICookbookBrowsingService cookbookBrowsingService)
        {
            _cookbookBrowsingService = cookbookBrowsingService;
        }

        public IActionResult Index()
        {
            AllCookbooksViewModel vm = _cookbookBrowsingService.CreateAllCookbooksViewModel();
            return View(vm);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
