using IdentityServer4.Services;
using ByLearning.Domain.Core.Bus;
using ByLearning.Domain.Core.Bus.Abstract;
using ByLearning.Domain.Core.Interfaces;
using ByLearning.SSO.Application.CloudServices.Storage;
using ByLearning.SSO.Application.Configuration;
using ByLearning.SSO.Application.Interfaces;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Microsoft.Extensions.DependencyInjection
{
#pragma warning disable S101 // Types should be named in PascalCase
    public static class SSOBootstrapper
#pragma warning restore S101 // Types should be named in PascalCase
    {
        /// <summary>
        /// Configure SSO Services
        /// </summary>
        /// <typeparam name="THttpUser">Implementation of ISystemUser</typeparam>
        /// <returns></returns>
        public static IServiceCollection ConfigureSSO<THttpUser>(this IServiceCollection services)
            where THttpUser : class, ISystemUser
        {
            services.TryAddScoped<IEventBus, InMemoryBus>();
            services.TryAddScoped<ISystemUser, THttpUser>();
            services.AddScoped<IEventSink, IdentityServerEventStore>();
            services.AddScoped<IStorage, StorageService>();

            services
                .AddApplicationServices()
                .AddDomainEvents()
                .AddDomainCommands();

            return services;
        }

        /// <summary>   
        /// Configure SSO Services
        /// </summary>
        /// <typeparam name="THttpUser">Implementation of ISystemUser</typeparam>
        /// <returns></returns>
        //public static IServiceCollection ConfigureSSO_1<THttpUser>(this IServiceCollection services)
        //    where THttpUser : class, ISystemUser
        //{
        //    services.TryAddScoped<IEventBus, InMemoryBus>();
        //    services.TryAddScoped<ISystemUser, THttpUser>();
        //    services.AddScoped<IEventSink, IdentityServerEventStore>();
        //    services.AddScoped<IStorage, StorageService>();
        //    services
        //        .AddApplicationServices()
        //        .AddDomainEvents()
        //        .AddDomainCommands();

        //    return services;
        //}
    }
}
