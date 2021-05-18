using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RecipePal.Models;
using RecipePal.Models.Identity;
using RecipePal.Repositories;
using RecipePal.ViewModels;
using System.Linq;
using System.Threading.Tasks;

using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace RecipePal.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        readonly UserManager<AppUser> _userManager;
        readonly SignInManager<AppUser> _signInManager;
        readonly IRepositoryFactory _repositoryFactory;

        public AccountController(UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager, IRepositoryFactory repositoryFactory)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _repositoryFactory = repositoryFactory;
        }

        public async Task<IActionResult> Index()
        {
            IRepository<Profile> profilesRepo = _repositoryFactory.CreateRepository<Profile>();

            if (_signInManager.IsSignedIn(User))
            {
                AppUser appUser = await _userManager.GetUserAsync(User);
                Profile profile = profilesRepo.Get().FirstOrDefault(p => p.Email == appUser.Email);
                return View(new ProfileViewModel
                {
                    Profile = profile
                });
            }
            return RedirectToAction(nameof(Create));
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
            IRepository<Profile> profilesRepo = _repositoryFactory.CreateRepository<Profile>();

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
                    profilesRepo.Insert(new Profile
                    {
                        Email = appUser.Email,
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
