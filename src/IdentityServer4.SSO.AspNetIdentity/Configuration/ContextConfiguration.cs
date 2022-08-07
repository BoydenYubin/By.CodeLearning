using ByLearning.Domain.Core.Interfaces;
using ByLearning.SSO.AspNetIdentity.Models.Identity;
using ByLearning.SSO.AspNetIdentity.Services;
using ByLearning.SSO.Domain.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace ByLearning.SSO.AspNetIdentity.Configuration
{
    public static class IdentityConfiguration
    {
        public static IServiceCollection ConfigureIdentity<TUser, TRole, TKey, TRoleFactory, TUserFactory>(this IServiceCollection services)
            where TUser : IdentityUser<TKey>, IDomainUser
            where TRole : IdentityRole<TKey>
            where TKey : IEquatable<TKey>
            where TRoleFactory : class, IRoleFactory<TRole>
            where TUserFactory : class, IIdentityFactory<TUser>
        {
            services.AddScoped<IUserService, UserService<TUser, TRole, TKey>>();
            services.AddScoped<IRoleService, RoleService<TRole, TKey>>();
            services.AddScoped<IIdentityFactory<TUser>, TUserFactory>();
            services.AddScoped<IRoleFactory<TRole>, TRoleFactory>();

            return services;
        }

        public static IServiceCollection AddDefaultAspNetIdentityServices(this IServiceCollection services)
        {
            // Infra - Identity Services
            services.AddScoped<IUserService, UserService<UserIdentity, RoleIdentity, string>>();
            services.AddScoped<IRoleService, RoleService<RoleIdentity, string>>();
            services.AddScoped<IIdentityFactory<UserIdentity>, IdentityFactory>();
            services.AddScoped<IRoleFactory<RoleIdentity>, IdentityFactory>();

            return services;
        }

    }
}
