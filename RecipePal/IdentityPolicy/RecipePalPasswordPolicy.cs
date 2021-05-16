using Microsoft.AspNetCore.Identity;
using RecipePal.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RecipePal.IdentityPolicy
{
    public class RecipePalPasswordPolicy : PasswordValidator<AppUser>
    {
        public override async Task<IdentityResult> ValidateAsync(
            UserManager<AppUser> manager, AppUser user, string password)
        {
            IdentityResult validationResult = await base.ValidateAsync(manager, user, password);
            var validationErrors = validationResult.Succeeded ?
                new List<IdentityError>() : validationResult.Errors.ToList();

            if (password.ToLower().Contains(user.UserName.ToLower()))
                validationErrors.Add(new IdentityError
                {
                    Description = "Password cannot contain username"
                });

            return validationErrors.Count == 0 ?
                IdentityResult.Success : IdentityResult.Failed(validationErrors.ToArray());
        }
    }
}
