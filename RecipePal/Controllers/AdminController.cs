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
        readonly IUserValidator<AppUser> _userValidator;
        readonly IPasswordValidator<AppUser> _passwordValidator;

        public AdminController(UserManager<AppUser> userManager, IUserValidator<AppUser> userValidator,
            IPasswordHasher<AppUser> passwordHasher, IPasswordValidator<AppUser> passwordValidator)
        {
            _userManager = userManager;
            _passwordHasher = passwordHasher;
            _userValidator = userValidator;
            _passwordValidator = passwordValidator;
        }

        public IActionResult Index()
        {
            return View(_userManager.Users);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            AppUser appUser = await _userManager.FindByIdAsync(id);
            if (appUser != null)
            {
                IdentityResult deleteResult = await _userManager.DeleteAsync(appUser);
                if (!deleteResult.Succeeded)
                    foreach (IdentityError err in deleteResult.Errors)
                        ModelState.AddModelError("", err.Description);
            }
            else ModelState.AddModelError("", "User not found");

            return RedirectToAction(nameof(Index));
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
                bool updatedEmail = await TryUpdateEmail(email, appUser);
                bool updatedPassword = await TryUpdatePassword(password, appUser);

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

        async Task<bool> TryUpdatePassword(string password, AppUser appUser)
        {
            if (!string.IsNullOrEmpty(password))
            {
                IdentityResult validationResult = await _passwordValidator
                    .ValidateAsync(_userManager, appUser, password);

                if (validationResult.Succeeded)
                {
                    appUser.PasswordHash = _passwordHasher.HashPassword(appUser, password);
                    return true;
                }
                else foreach (IdentityError err in validationResult.Errors)
                        ModelState.AddModelError("", err.Description);
            }
            else ModelState.AddModelError("", "Password cannot be empty");
            return false;
        }

        async Task<bool> TryUpdateEmail(string email, AppUser appUser)
        {
            if (!string.IsNullOrEmpty(email))
            {
                IdentityResult validationResult = await _userValidator
                    .ValidateAsync(_userManager, appUser);

                if (validationResult.Succeeded)
                {
                    appUser.Email = email;
                    return true;
                }
                else foreach (IdentityError err in validationResult.Errors)
                        ModelState.AddModelError("", err.Description);
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
