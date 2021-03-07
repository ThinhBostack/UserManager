using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserManager.Models;

namespace UserManager.Policies
{
    //This class is for custom policies of attibute of identity user 
    public class CustomPolicy : UserValidator<User>
    {
        public override async Task<IdentityResult> ValidateAsync(UserManager<User> manager, User user)
        {
            IdentityResult result = await base.ValidateAsync(manager, user);
            List<IdentityError> errors = result.Succeeded ? new List<IdentityError>() : result.Errors.ToList();

            if (user.UserName == "google")
            {
                errors.Add(new IdentityError
                {
                    Description = "Google cannot be used as a user name"
                });
            }

            if (!user.Email.ToLower().EndsWith("@gmail.com"))
            {
                errors.Add(new IdentityError
                {
                    Description = "Only gmail.com email addresses are allowed"
                });
            }
            return errors.Count == 0 ? IdentityResult.Success : IdentityResult.Failed(errors.ToArray());
        }
    }
}
