using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RecipePal.Models;
using System.Threading.Tasks;

namespace RecipePal.Controllers
{
    public class AdminController : Controller
    {
        readonly UserManager<AppUser> _userManager;

        public AdminController(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            return View(_userManager.Users);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Chef chef)
        {
            if (ModelState.IsValid)
            {
                var appUser = new AppUser
                {
                    UserName = chef.UserName,
                    Email = chef.Email
                };

                IdentityResult result = await _userManager.CreateAsync(appUser, chef.Password);
                if (result.Succeeded)
                    return RedirectToAction(nameof(Index));

                foreach (IdentityError err in result.Errors)
                    ModelState.AddModelError("", err.Description);
            }

            return View(chef);
        }
    }
}
