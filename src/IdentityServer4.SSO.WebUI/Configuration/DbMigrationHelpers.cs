using ByLearning.EntityFrameworkCore.MigrationHelper;
using ByLearning.SSO.AspNetIdentity.Models.Identity;
using IdentityServer4.EntityFramework.Mappers;
using IdentityServer4.SSO.Database.Context;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Linq;
using System.Security.Claims;
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
                await EnsureSeedIdentityServerData(ssoContext, configuration);
                await EnsureSeedIdentityData(userManager, roleManager, configuration);
                //await EnsureSeedGlobalConfigurationData(ssoContext, configuration, env);
            }
        }

        /// <summary>
        /// Generate default clients, identity and api resources
        /// </summary>
        private static async Task EnsureSeedIdentityServerData(SSOContext context, IConfiguration configuration)
        {
            if (!context.Clients.Any())
            {
                foreach (var client in Clients.GetAdminClient(configuration).ToList())
                {
                    await context.Clients.AddAsync(client.ToEntity());
                }

                await context.SaveChangesAsync();
            }

            if (!context.IdentityResources.Any())
            {
                var identityResources = ClientResources.GetIdentityResources().ToList();

                foreach (var resource in identityResources)
                {
                    await context.IdentityResources.AddAsync(resource.ToEntity());
                }

                await context.SaveChangesAsync();
            }

            if (!context.ApiResources.Any())
            {
                foreach (var resource in ClientResources.GetApiResources().ToList())
                {
                    await context.ApiResources.AddAsync(resource.ToEntity());
                }

                await context.SaveChangesAsync();
            }
        }

        /// <summary>
        /// Generate default admin user / role
        /// </summary>
        private static async Task EnsureSeedIdentityData(
            UserManager<UserIdentity> userManager,
            RoleManager<RoleIdentity> roleManager,
            IConfiguration configuration)
        {

            // Create admin role
            if (!await roleManager.RoleExistsAsync("Administrator"))
            {
                var role = new RoleIdentity { Name = "Administrator" };

                await roleManager.CreateAsync(role);
            }

            // Create admin user
            if (await userManager.FindByNameAsync(Users.GetUser(configuration)) != null) return;

            var user = new UserIdentity
            {
                UserName = Users.GetUser(configuration),
                Email = Users.GetEmail(configuration),
                EmailConfirmed = true,
                LockoutEnd = null
            };

            var result = await userManager.CreateAsync(user, Users.GetPassword(configuration));

            if (result.Succeeded)
            {
                await userManager.AddClaimAsync(user, new Claim("is4-rights", "manager"));
                await userManager.AddClaimAsync(user, new Claim("username", Users.GetUser(configuration)));
                await userManager.AddClaimAsync(user, new Claim("email", Users.GetEmail(configuration)));
                await userManager.AddToRoleAsync(user, "Administrator");
            }
        }
    }
}
