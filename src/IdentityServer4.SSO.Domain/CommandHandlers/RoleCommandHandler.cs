using ByLearning.Domain.Core.Bus.Abstract;
using ByLearning.Domain.Core.Commands;
using ByLearning.Domain.Core.Interfaces;
using ByLearning.Domain.Core.Notifications;
using ByLearning.SSO.Domain.Commands.Role;
using ByLearning.SSO.Domain.Events.Role;
using ByLearning.SSO.Domain.Interfaces;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace ByLearning.SSO.Domain.CommandHandlers
{
    public class RoleCommandHandler : CommandHandler,
        IRequestHandler<RemoveRoleCommand, bool>,
        IRequestHandler<SaveRoleCommand, bool>,
        IRequestHandler<UpdateRoleCommand, bool>,
        IRequestHandler<RemoveUserFromRoleCommand, bool>
    {
        private readonly IRoleService _roleService;
        private readonly IUserService _userService;

        public RoleCommandHandler(
            IUnitOfWork uow,
            IEventBus bus,
            INotificationHandler<DomainNotification> notifications,
            IRoleService roleService,
            IUserService userService) : base(uow, bus, notifications)
        {
            _roleService = roleService;
            _userService = userService;
        }

        public async Task<bool> Handle(RemoveRoleCommand request, CancellationToken cancellationToken)
        {
            if (!request.IsValid())
            {
                NotifyValidationErrors(request);
                return false;
            }

            // Businness logic here
            var result = await _roleService.Remove(request.Name);

            if (result)
            {
                await Bus.Publish(new RoleRemovedEvent(request.Name));
                return true;
            }
            return false;
        }

        public async Task<bool> Handle(SaveRoleCommand request, CancellationToken cancellationToken)
        {
            if (!request.IsValid())
            {
                NotifyValidationErrors(request);
                return false;
            }

            // Businness logic here
            var result = await _roleService.Save(request.Name);

            if (result)
            {
                await Bus.Publish(new RoleSavedEvent(request.Name));
                return true;
            }
            return false;
        }

        public async Task<bool> Handle(UpdateRoleCommand request, CancellationToken cancellationToken)
        {
            if (!request.IsValid())
            {
                NotifyValidationErrors(request);
                return false;
            }

            // Businness logic here
            var result = await _roleService.Update(request.Name, request.OldName);

            if (result)
            {
                await Bus.Publish(new RoleUpdatedEvent(request.Name, request.OldName));
                return true;
            }
            return false;
        }

        public async Task<bool> Handle(RemoveUserFromRoleCommand request, CancellationToken cancellationToken)
        {
            if (!request.IsValid())
            {
                NotifyValidationErrors(request);
                return false;
            }

            var result = await _userService.RemoveUserFromRole(request.Name, request.Username);

            if (result)
            {
                await Bus.Publish(new UserRemovedFromRoleEvent(request.Name, request.Username));
                return true;
            }
            return false;
        }
    }
}
