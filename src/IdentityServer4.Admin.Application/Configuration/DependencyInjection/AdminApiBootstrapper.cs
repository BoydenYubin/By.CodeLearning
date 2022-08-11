using ByLearning.Admin.Application.Configuration;
using ByLearning.Domain.Core.Bus;
using ByLearning.Domain.Core.Bus.Abstract;
using ByLearning.Domain.Core.Interfaces;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class AdminApiBootstrapper
    {
        /// <summary>
        /// Configure IdentityServer4 Administration services
        /// </summary>
        public static IServiceCollection ConfigureAdminServices<TUser>(this IServiceCollection services)
            where TUser : class, ISystemUser
        {
            // Domain Bus (Mediator)
            services.TryAddScoped<IEventBus, InMemoryBus>();
            services.TryAddScoped<ISystemUser, TUser>();
            services
                .AddApplicationServices()
                .AddDomainEventsServices()
                .AddDomainCommandsServices();

            return services;
        }
    }

}