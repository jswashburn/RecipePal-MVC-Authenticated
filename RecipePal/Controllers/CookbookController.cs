using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RecipePal.Models;
using RecipePal.Repositories;
using RecipePal.Services;
using RecipePal.ViewModels;

namespace RecipePal.Controllers
{
    [Authorize]
    public class CookbookController : Controller
    {
        readonly IRepositoryFactory _repositoryFactory;
        readonly ICookbookBrowsingService _cookbookBrowsingService;

        public CookbookController(ICookbookBrowsingService cookbookBrowsingService,
            IRepositoryFactory repositoryFactory)
        {
            _cookbookBrowsingService = cookbookBrowsingService;
            _repositoryFactory = repositoryFactory;
        }

        [AllowAnonymous]
        public IActionResult Index(int id)
        {
            CookbookViewModel vm = _cookbookBrowsingService.CreateCookbookViewModel(id);
            return View(vm);
        }

        public IActionResult Delete(int cookbookId, string redirectUrl = "/")
        {
            IRepository<Cookbook> cookbooksRepo = _repositoryFactory.CreateRepository<Cookbook>();

            cookbooksRepo.Delete(cookbookId);

            return Redirect(redirectUrl);
        }
    }
}
