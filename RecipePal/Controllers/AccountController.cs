﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RecipePal.Models;
using System.Threading.Tasks;

using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace RecipePal.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        readonly UserManager<AppUser> _userManager;
        readonly SignInManager<AppUser> _signInManager;

        public AccountController(UserManager<AppUser> userManager, 
            SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        [AllowAnonymous]
        public IActionResult Login(string returnUrl)
        {
            Login login = new Login
            {
                ReturnUrl = returnUrl
            };
            return View(login);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(Login login)
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