using IdentityServer4.EntityFramework.Interfaces;
using IdentityServer4.EntityFramework.Options;
using ByLearning.Admin.Domain.Interfaces;
using ByLearning.Admin.EntityFramework.Repository.Context;
using ByLearning.Admin.EntityFramework.Repository.Repository;
using ByLearning.Domain.Core.Events;
using ByLearning.Domain.Core.Interfaces;
using ByLearning.EntityFrameworkCore.EventSourcing;
using ByLearning.EntityFrameworkCore.Interfaces;
using ByLearning.EntityFrameworkCore.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ContextConfiguration
    {

        public static IServiceCollection ConfigureJpAdminStorageServices(this IServiceCollection services)
        {
            RegisterStorageServices(services);
            return services;
        }

        public static IServiceCollection SetupDefaultIdentityServerContext<TContext>(
            this IServiceCollection services)
            where TContext : IPersistedGrantDbContext, IConfigurationDbContext
        {
            // Configure identityserver4 default database
            var operationalStoreOptions = new OperationalStoreOptions();
            var storeOptions = new ConfigurationStoreOptions();

            services.AddSingleton(operationalStoreOptions);
            services.AddSingleton(storeOptions);
            services.TryAddScoped<IPersistedGrantDbContext>(x => x.GetRequiredService<TContext>());
            services.TryAddScoped<IConfigurationDbContext>(x => x.GetRequiredService<TContext>());
            return services;
        }

        public static IServiceCollection AddJpAdminContext(this IServiceCollection services, Action<DbContextOptionsBuilder> optionsAction, JpDatabaseOptions options = null)
        {
            if (options == null)
                options = new JpDatabaseOptions();

            RegisterStorageServices(services);

            services.TryAddScoped<IUnitOfWork, UnitOfWork>();

            services.AddDbContext<IdentityServer4AdminUiContext>(optionsAction);
            services.AddScoped<IEntityFrameworkStore>(x => x.GetService<IdentityServer4AdminUiContext>());
            services.AddScoped<IConfigurationDbContext>(x => x.GetService<IdentityServer4AdminUiContext>());
            services.AddScoped<IPersistedGrantDbContext>(x => x.GetService<IdentityServer4AdminUiContext>());
            services.SetupDefaultIdentityServerContext<IdentityServer4AdminUiContext>();
            return services;
        }

        public static IServiceCollection ConfigureAdminContext<TContext>(this IServiceCollection services)
            where TContext : class, IPersistedGrantDbContext, IConfigurationDbContext, IEntityFrameworkStore, IEventStoreContext
        {
            RegisterStorageServices(services);

            services.TryAddScoped<IUnitOfWork, UnitOfWork>();
            services.TryAddScoped<IEntityFrameworkStore, TContext>(); services.TryAddScoped<IPersistedGrantDbContext>(x => x.GetRequiredService<TContext>());
            services.TryAddScoped<IConfigurationDbContext>(x => x.GetRequiredService<TContext>());
            services.TryAddScoped<IEventStoreContext>(x => x.GetRequiredService<TContext>());

            return services;
        }

        public static IServiceCollection ConfigureAdminContext<TContext, TEventStore>(this IServiceCollection services)
            where TContext : class, IPersistedGrantDbContext, IConfigurationDbContext, IEntityFrameworkStore
            where TEventStore : class, IEventStoreContext
        {
            RegisterStorageServices(services);
            services.TryAddScoped<IEventStoreContext>(x => x.GetRequiredService<TEventStore>());
            services.TryAddScoped<IConfigurationDbContext>(x => x.GetRequiredService<TContext>());
            services.TryAddScoped<IPersistedGrantDbContext>(x => x.GetRequiredService<TContext>());
            services.TryAddScoped<IEntityFrameworkStore>(x => x.GetRequiredService<TContext>());

            return services;
        }


        private static void RegisterStorageServices(IServiceCollection services)
        {
            services.AddScoped<IPersistedGrantRepository, PersistedGrantRepository>();
            services.AddScoped<IApiResourceRepository, ApiResourceRepository>();
            services.AddScoped<IIdentityResourceRepository, IdentityResourceRepository>();
            services.AddScoped<IClientRepository, ClientRepository>();
            services.AddScoped<IEventStoreRepository, EventStoreRepository>();
            services.TryAddScoped<IEventStoreService, SqlEventStore>();
        }

        public static IServiceCollection AddEventStore<TEventStore>(this IServiceCollection services)
            where TEventStore : class, IEventStoreContext
        {
            services.AddScoped<IEventStoreContext, TEventStore>();
            return services;
        }
    }
}
