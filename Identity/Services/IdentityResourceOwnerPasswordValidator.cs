﻿using System.Collections.Generic;
using System.Threading.Tasks;
using IdentityModel;
using IdentityServer4.Validation;
using Microsoft.AspNetCore.Identity;
using UdemyMicroservices.Identity.Models;

namespace UdemyMicroservices.Identity.Services
{
    public class IdentityResourceOwnerPasswordValidator : IResourceOwnerPasswordValidator
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public IdentityResourceOwnerPasswordValidator(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task ValidateAsync(ResourceOwnerPasswordValidationContext context)
        {
            var existUser = await _userManager.FindByEmailAsync(context.UserName);

            if (existUser == null)
            {
                var errors = new Dictionary<string, object>
                {
                    {"errors", new List<string> {"Email veya şifreniz yanlış"}}
                };
                context.Result.CustomResponse = errors;

                return;
            }
            var passwordCheck = await _userManager.CheckPasswordAsync(existUser, context.Password);

            if (passwordCheck == false)
            {
                var errors = new Dictionary<string, object>
                {
                    {"errors", new List<string> {"Email veya şifreniz yanlış"}}
                };
                context.Result.CustomResponse = errors;

                return;
            }

            context.Result = new GrantValidationResult(existUser.Id, OidcConstants.AuthenticationMethods.Password);
        }
    }
}
