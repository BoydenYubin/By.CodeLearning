using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;

namespace IdentityServer4.Admin.WebAPI.Configuration
{
    public static class ConfigurePolicy
    {
        public static void AddPolicies(this IServiceCollection services)
        {
            services.AddAuthorization(options =>
            {
                options.AddPolicy("UserManagement", policy =>
                    policy.RequireAuthenticatedUser());

                options.AddPolicy("Default",
                    policy => policy.RequireRole("Administrator")
                    .Requirements.Add(new AccountRequirement()));
            });
            services.AddSingleton<IAuthorizationHandler, AccountRequirementHandler>();
        }
    }
}
