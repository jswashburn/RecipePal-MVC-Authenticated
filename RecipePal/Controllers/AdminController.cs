using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RecipePal.Models;
using System.Threading.Tasks;

namespace RecipePal.Controllers
{
    public class AdminController : Controller
    {
        readonly UserManager<AppUser> _userManager;
        readonly IPasswordHasher<AppUser> _passwordHasher;

        public AdminController(UserManager<AppUser> userManager, 
            IPasswordHasher<AppUser> passwordHasher)
        {
            _userManager = userManager;
            _passwordHasher = passwordHasher;
        }

        public IActionResult Index()
        {
            return View(_userManager.Users);
        }

        public async Task<IActionResult> Update(string id)
        {
            AppUser appUser = await _userManager.FindByIdAsync(id);
            if (appUser == null)
                return RedirectToAction(nameof(Index));
            return View(appUser);
        }

        [HttpPost]
        public async Task<IActionResult> Update(string id, string email, string password)
        {
            AppUser appUser = await _userManager.FindByIdAsync(id);

            if (appUser != null)
            {
                bool updatedEmail = TryUpdateEmail(email, appUser);
                bool updatedPassword = TryUpdatePassword(password, appUser);

                if (updatedEmail && updatedPassword)
                {
                    IdentityResult updateResult = await _userManager.UpdateAsync(appUser);
                    if (updateResult.Succeeded)
                        return RedirectToAction(nameof(Index));
                    else foreach (IdentityError err in updateResult.Errors)
                            ModelState.AddModelError("", err.Description);
                }
            }
            else ModelState.AddModelError("", "User not found");

            return View(appUser);
        }

        bool TryUpdatePassword(string password, AppUser appUser)
        {
            if (!string.IsNullOrEmpty(password))
            {
                appUser.PasswordHash = _passwordHasher.HashPassword(appUser, password);
                return true;
            }
            else ModelState.AddModelError("", "Password cannot be empty");
            return false;
        }

        bool TryUpdateEmail(string email, AppUser appUser)
        {
            if (!string.IsNullOrEmpty(email))
            {
                appUser.Email = email;
                return true;
            }
            else ModelState.AddModelError("", "Email cannot be empty");
            return false;
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
