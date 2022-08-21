using ByLearning.SSO.AspNetIdentity.Configuration;
using ByLearning.SSO.AspNetIdentity.Models.Identity;
using ByLearning.SSO.EntityFramework.Repository.Configuration;
using IdentityServer4.SSO.AspNetIdentity;
using IdentityServer4.SSO.Database;
using IdentityServer4.SSO.Database.Context;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Net;
using System.Threading.Tasks;
using static Microsoft.Extensions.Configuration.ProviderSelector;
using IConfiguration = Microsoft.Extensions.Configuration.IConfiguration;

namespace IdentityServer4.Admin.WebAPI.Configuration
{
    public static class SSOApiConfiguration
    {
        public static IServiceCollection ConfigureSSOApi(this IServiceCollection services, IConfiguration configuration)
        {
            services.ConfigureProviderForContext<SSOContext>(DetectDatabase(configuration));


            //// ASP.NET Identity Configuration
            services
                .AddIdentity<UserIdentity, RoleIdentity>(AccountOptions.NistAccountOptions)
                .AddEntityFrameworkStores<SSOContext>()
                .AddDefaultTokenProviders(); ;

            //// SSO Services
            services
                .ConfigureSSO<AspNetUser>()
                .AddSSOContext<SSOContext>()
                .AddDefaultAspNetIdentityServices();

            //// IdentityServer4 Admin services
            services
                .ConfigureAdminServices<AspNetUser>()
                .ConfigureJpAdminStorageServices()
                .SetupDefaultIdentityServerContext<SSOContext>();


            services.UpgradePasswordSecurity().UseArgon2<UserIdentity>();

            SetupGeneralAuthorizationSettings(services);

            return services;
        }


        private static void SetupGeneralAuthorizationSettings(IServiceCollection services)
        {
            services.ConfigureApplicationCookie(options =>
            {
                //options.AccessDeniedPath = new PathString("/accounts/access-denied");
                options.Events.OnRedirectToAccessDenied = context =>
                {
                    context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                    return Task.CompletedTask;
                };
            });
        }
        /// <summary>
        /// it's just a tuple. Returns 2 parameters.
        /// Trying to improve readability at ConfigureServices
        /// </summary>
        private static (DatabaseType, string) DetectDatabase(IConfiguration configuration) => (
            configuration.GetValue<DatabaseType>("ApplicationSettings:DatabaseType"),
            configuration.GetConnectionString("SSOConnection"));
    }
}
