using Microsoft.AspNetCore.Identity;
using RecipePal.Models.Identity;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProfanityDetector.Interfaces;

namespace RecipePal.IdentityPolicy
{
    public class RecipePalUserPolicy : UserValidator<AppUser>
    {
        readonly IProfanityFilter _profanityFilter;

        public RecipePalUserPolicy(IProfanityFilter profanityFilter)
        {
            _profanityFilter = profanityFilter;
        }

        public override async Task<IdentityResult> ValidateAsync(
            UserManager<AppUser> userManager, AppUser appUser)
        {
            IdentityResult validationResult = await base.ValidateAsync(userManager, appUser);
            var validationErrors = validationResult.Succeeded ?
                new List<IdentityError>() : validationResult.Errors.ToList();

            bool isProfane = _profanityFilter.ContainsProfanity(appUser.UserName);

            if (isProfane)
                validationErrors.Add(new IdentityError
                {
                    Description = "Profanity detected in username"
                });

            return validationErrors.Count == 0 ?
                IdentityResult.Success : IdentityResult.Failed(validationErrors.ToArray());
        }
    }
}
