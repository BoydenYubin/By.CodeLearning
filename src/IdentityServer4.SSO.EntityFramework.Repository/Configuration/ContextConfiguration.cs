using ByLearning.Domain.Core.Events;
using ByLearning.Domain.Core.Interfaces;
using ByLearning.EntityFrameworkCore.EventSourcing;
using ByLearning.EntityFrameworkCore.Interfaces;
using ByLearning.EntityFrameworkCore.Repository;
using ByLearning.SSO.EntityFramework.Repository.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace ByLearning.SSO.EntityFramework.Repository.Configuration
{
    public static class ContextConfiguration
    {
        public static IServiceCollection AddSSOContext<TContext, TEventStore>(this IServiceCollection services)
            where TContext : class, ISSOContext
            where TEventStore : class, IEventStoreContext

        {
            services.TryAddScoped<IEventStoreService, SqlEventStore>();
            services.TryAddScoped<IUnitOfWork, UnitOfWork>();
            services.TryAddScoped<IEventStoreContext, TEventStore>();
            services.AddScoped<ISSOContext, TContext>();
            services.TryAddScoped<IEntityFrameworkStore>(x => x.GetRequiredService<TContext>());
            services.AddStores();
            return services;
        }
        public static IServiceCollection AddSSOContext<TContext>(this IServiceCollection services)
            where TContext : class, ISSOContext, IEventStoreContext

        {
            services.TryAddScoped<IEventStoreService, SqlEventStore>();
            services.TryAddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<ISSOContext, TContext>();
            services.TryAddScoped<IEventStoreContext>(s => s.GetService<TContext>());
            services.TryAddScoped<IEntityFrameworkStore>(x => x.GetRequiredService<TContext>());
            services.AddStores();
            return services;
        }
    }
}
