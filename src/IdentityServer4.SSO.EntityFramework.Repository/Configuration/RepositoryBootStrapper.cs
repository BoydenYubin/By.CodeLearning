using ByLearning.Domain.Core.Interfaces;
using ByLearning.EntityFrameworkCore.Repository;
using ByLearning.SSO.Domain.Interfaces;
using ByLearning.SSO.EntityFramework.Repository.Repository;
using Microsoft.Extensions.DependencyInjection;

namespace ByLearning.SSO.EntityFramework.Repository.Configuration
{
    internal static class RepositoryBootStrapper
    {
        public static IServiceCollection AddStores(this IServiceCollection services)
        {
            // Infra - Data EventSourcing
            services.AddScoped<IEventStoreRepository, EventStoreRepository>();
            services.AddScoped<ITemplateRepository, TemplateRepository>();
            services.AddScoped<IGlobalConfigurationSettingsRepository, GlobalConfigurationSettingsRepository>();
            services.AddScoped<IEmailRepository, EmailRepository>();

            return services;
        }
    }
}
