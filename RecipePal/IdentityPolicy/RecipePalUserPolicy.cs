using Microsoft.AspNetCore.Identity;
using RecipePal.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RecipePal.IdentityPolicy
{
    public class RecipePalUserPolicy : UserValidator<AppUser>
    {
        public override async Task<IdentityResult> ValidateAsync(
            UserManager<AppUser> userManager, AppUser appUser)
        {
            IdentityResult validationResult = await base.ValidateAsync(userManager, appUser);
            var validationErrors = validationResult.Succeeded ?
                new List<IdentityError>() : validationResult.Errors.ToList();

            if (appUser.UserName == "wwishy")
                validationErrors.Add(new IdentityError
                {
                    Description = "Can't have that name its already mine >:("
                });

            if (appUser.NormalizedEmail.EndsWith("boogle.com"))
                validationErrors.Add(new IdentityError
                {
                    Description = "Can't use boogle emails"
                });

            return validationErrors.Count == 0 ?
                IdentityResult.Success : IdentityResult.Failed(validationErrors.ToArray());
        }
    }
}
