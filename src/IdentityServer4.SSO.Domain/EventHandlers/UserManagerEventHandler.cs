﻿using System.Threading;
using System.Threading.Tasks;
using ByLearning.SSO.Domain.Events.UserManagement;
using MediatR;

namespace ByLearning.SSO.Domain.EventHandlers
{
    public class UserManagerEventHandler : 
        INotificationHandler<ProfileUpdatedEvent>,
        INotificationHandler<ProfilePictureUpdatedEvent>,
        INotificationHandler<PasswordCreatedEvent>,
        INotificationHandler<PasswordChangedEvent>,
        INotificationHandler<AccountRemovedEvent>
    {
        public Task Handle(ProfileUpdatedEvent notification, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        public Task Handle(ProfilePictureUpdatedEvent notification, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        public Task Handle(PasswordCreatedEvent notification, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        public Task Handle(PasswordChangedEvent notification, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        public Task Handle(AccountRemovedEvent notification, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
