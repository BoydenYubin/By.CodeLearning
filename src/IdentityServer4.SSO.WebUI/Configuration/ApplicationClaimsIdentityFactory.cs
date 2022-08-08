﻿using ByLearning.SSO.AspNetIdentity.Models.Identity;
using ByLearning.SSO.Domain.ViewModels;
using IdentityServer4.SSO.WebUI.Util;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace IdentityServer4.SSO.WebUI.Configuration
{
    public class ApplicationClaimsIdentityFactory : UserClaimsPrincipalFactory<UserIdentity>
    {
        public ApplicationClaimsIdentityFactory(UserManager<UserIdentity> userManager, IOptions<IdentityOptions> optionsAccessor) : base(userManager, optionsAccessor)
        {
        }

        protected override async Task<ClaimsIdentity> GenerateClaimsAsync(UserIdentity user)
        {
            var identity = await base.GenerateClaimsAsync(user);
            var claims = new List<Claim>();

            claims.AddIfDontExist(new Claim(JwtClaimTypes.Name, user.UserName));
            claims.AddIfDontExist(new Claim(JwtClaimTypes.GivenName, user.UserName));
            var roles = await UserManager.GetRolesAsync(user);

            if (identity.Claims.All(c => c.Type != JwtClaimTypes.Role))
                claims.AddRange(roles.Select(s => new Claim(JwtClaimTypes.Role, s)));

            identity.AddClaims(claims);
            return identity;
        }
    }
}
