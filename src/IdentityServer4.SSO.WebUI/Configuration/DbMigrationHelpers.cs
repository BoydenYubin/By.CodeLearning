using ByLearning.EntityFrameworkCore.MigrationHelper;
using ByLearning.SSO.AspNetIdentity.Models.Identity;
using IdentityServer4.SSO.Database.Context;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading.Tasks;

namespace IdentityServer4.SSO.WebUI.Configuration
{
    public static class DbMigrationHelpers
    {
        /// <summary>
        /// Generate migrations before running this method, you can use command bellow:
        /// Nuget package manager: Add-Migration DbInit -context ApplicationIdentityContext -output Data/Migrations
        /// Dotnet CLI: dotnet ef migrations add DbInit -c ApplicationIdentityContext -o Data/Migrations
        /// </summary>
        public static async Task EnsureSeedData(IServiceScope serviceScope)
        {
            var services = serviceScope.ServiceProvider;
            await EnsureSeedData(services);
        }

        public static async Task EnsureSeedData(IServiceProvider serviceProvider)
        {
            using (var scope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var env = scope.ServiceProvider.GetRequiredService<IWebHostEnvironment>();

                var configuration = scope.ServiceProvider.GetRequiredService<IConfiguration>();
                var ssoContext = scope.ServiceProvider.GetRequiredService<SSOContext>();
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<UserIdentity>>();
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<RoleIdentity>>();

                await DbHealthChecker.TestConnection(ssoContext);

                if (env.IsDevelopment())
                    ssoContext.Database.EnsureCreated();

                //暂时注释掉相关seed数据
                //await EnsureSeedIdentityServerData(ssoContext, configuration);
                //await EnsureSeedIdentityData(userManager, roleManager, configuration);
                //await EnsureSeedGlobalConfigurationData(ssoContext, configuration, env);
            }
        }
    }
}
