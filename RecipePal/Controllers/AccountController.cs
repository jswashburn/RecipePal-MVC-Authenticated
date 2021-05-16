using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RecipePal.Models;
using RecipePal.Models.Identity;
using RecipePal.Repositories;
using RecipePal.ViewModels;
using System.Threading.Tasks;

using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace RecipePal.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        readonly UserManager<AppUser> _userManager;
        readonly SignInManager<AppUser> _signInManager;
        readonly IRepository<Profile> _profilesRepo;

        public AccountController(UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager, IRepository<Profile> profiles)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _profilesRepo = profiles;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        [AllowAnonymous]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Create(LoginViewModel loginViewModel)
        {
            if (ModelState.IsValid)
            {
                var appUser = new AppUser
                {
                    UserName = loginViewModel.Profile.UserName,
                    Email = loginViewModel.Email
                };

                IdentityResult result = await _userManager.CreateAsync(appUser, loginViewModel.Password);
                if (result.Succeeded)
                {
                    _profilesRepo.Insert(new Profile 
                    { 
                        UserName = loginViewModel.Profile.UserName
                    });
                    return RedirectToAction(nameof(Index));
                }

                foreach (IdentityError err in result.Errors)
                    ModelState.AddModelError("", err.Description);
            }

            return View(loginViewModel);
        }

        [AllowAnonymous]
        public IActionResult Login(string returnUrl)
        {
            LoginViewModel login = new LoginViewModel
            {
                ReturnUrl = returnUrl
            };
            return View(login);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel login)
        {
            if (ModelState.IsValid)
            {
                AppUser appUser = await _userManager.FindByEmailAsync(login.Email);
                if (appUser != null)
                {
                    await _signInManager.SignOutAsync();
                    SignInResult signInResult = await _signInManager.PasswordSignInAsync(
                        user: appUser,
                        password: login.Password,
                        isPersistent: false,
                        lockoutOnFailure: false);

                    if (signInResult.Succeeded)
                        return Redirect(login.ReturnUrl ?? "/");
                }
                ModelState.AddModelError(nameof(login.Email),
                    "Login Failed: Invalid email or password");
            }
            return View(login);
        }
    }
}
