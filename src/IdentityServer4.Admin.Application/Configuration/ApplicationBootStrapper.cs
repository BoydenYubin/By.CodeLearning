using ByLearning.Admin.Application.Interfaces;
using ByLearning.Admin.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace ByLearning.Admin.Application.Configuration
{
    internal static class ApplicationBootStrapper
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IPersistedGrantAppService, PersistedGrantAppService>();
            services.AddScoped<IApiResourceAppService, ApiResourceAppService>();
            services.AddScoped<IIdentityResourceAppService, IdentityResourceAppService>();
            services.AddScoped<IIdentityServerEventStore, IdentityServerEventStoreAppService>();
            services.AddScoped<IScopesAppService, ScopesAppService>();
            services.AddScoped<IClientAppService, ClientAppService>();

            return services;
        }
    }
}
