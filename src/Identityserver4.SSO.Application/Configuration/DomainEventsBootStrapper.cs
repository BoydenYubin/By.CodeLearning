using ByLearning.Domain.Core.Notifications;
using ByLearning.SSO.Domain.EventHandlers;
using ByLearning.SSO.Domain.Events.User;
using ByLearning.SSO.Domain.Events.UserManagement;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace ByLearning.SSO.Application.Configuration
{
    internal static class DomainEventsBootStrapper
    {
        public static IServiceCollection AddDomainEvents(this IServiceCollection services)
        {
            services.TryAddScoped<INotificationHandler<DomainNotification>, DomainNotificationHandler>();

            services.AddScoped<INotificationHandler<UserRegisteredEvent>, UserEventHandler>();
            services.AddScoped<INotificationHandler<EmailConfirmedEvent>, UserEventHandler>();
            services.AddScoped<INotificationHandler<ProfileUpdatedEvent>, UserManagerEventHandler>();
            services.AddScoped<INotificationHandler<ProfilePictureUpdatedEvent>, UserManagerEventHandler>();
            services.AddScoped<INotificationHandler<PasswordCreatedEvent>, UserManagerEventHandler>();
            services.AddScoped<INotificationHandler<PasswordChangedEvent>, UserManagerEventHandler>();
            services.AddScoped<INotificationHandler<AccountRemovedEvent>, UserManagerEventHandler>();
            return services;
        }
    }
}
