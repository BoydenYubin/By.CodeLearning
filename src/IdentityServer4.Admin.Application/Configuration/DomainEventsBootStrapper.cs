using ByLearning.Domain.Core.Notifications;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace ByLearning.Admin.Application.Configuration
{
    internal static class DomainEventsBootStrapper
    {
        public static IServiceCollection AddDomainEventsServices(this IServiceCollection services)
        {
            services.TryAddScoped<INotificationHandler<DomainNotification>, DomainNotificationHandler>();
            return services;
        }
    }
}
