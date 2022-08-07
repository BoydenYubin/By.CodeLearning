using ByLearning.Domain.Core.Bus.Abstract;
using ByLearning.Domain.Core.Commands;
using ByLearning.Domain.Core.Interfaces;
using ByLearning.Domain.Core.Notifications;
using ByLearning.SSO.Domain.Commands.User;
using ByLearning.SSO.Domain.Commands.UserManagement;
using ByLearning.SSO.Domain.Events.User;
using ByLearning.SSO.Domain.Events.UserManagement;
using ByLearning.SSO.Domain.Interfaces;
using MediatR;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace ByLearning.SSO.Domain.CommandHandlers
{
    public class UserManagementCommandHandler : CommandHandler,
        IRequestHandler<UpdateProfileCommand, bool>,
        IRequestHandler<UpdateProfilePictureCommand, bool>,
        IRequestHandler<SetPasswordCommand, bool>,
        IRequestHandler<ChangePasswordCommand, bool>,
        IRequestHandler<RemoveAccountCommand, bool>,
        IRequestHandler<AdminUpdateUserCommand, bool>,
        IRequestHandler<SaveUserClaimCommand, bool>,
        IRequestHandler<RemoveUserClaimCommand, bool>,
        IRequestHandler<RemoveUserRoleCommand, bool>,
        IRequestHandler<SaveUserRoleCommand, bool>,
        IRequestHandler<AdminChangePasswordCommand, bool>,
        IRequestHandler<SynchronizeClaimsCommand, bool>
    {
        private readonly IUserService _userService;
        private readonly ISystemUser _user;

        public UserManagementCommandHandler(
            IUnitOfWork uow,
            IEventBus bus,
            INotificationHandler<DomainNotification> notifications,
            IUserService userService,
            ISystemUser user)
            : base(uow, bus, notifications)
        {
            _userService = userService;
            _user = user;
        }

        public async Task<bool> Handle(UpdateProfileCommand request, CancellationToken cancellationToken)
        {
            if (!request.IsValid())
            {
                NotifyValidationErrors(request);
                return false;
            }

            var result = await _userService.UpdateProfileAsync(request);
            if (result)
            {
                await Bus.Publish(new ProfileUpdatedEvent(request.Username, request));
                return true;
            }
            return false;
        }

        public async Task<bool> Handle(UpdateProfilePictureCommand request, CancellationToken cancellationToken)
        {
            if (!request.IsValid())
            {
                NotifyValidationErrors(request);
                return false;
            }

            var result = await _userService.UpdateProfilePictureAsync(request);
            if (result)
            {
                await Bus.Publish(new ProfilePictureUpdatedEvent(request.Username, request.Picture));
                return true;
            }
            return false;
        }

        public async Task<bool> Handle(SetPasswordCommand request, CancellationToken cancellationToken)
        {
            if (!request.IsValid())
            {
                NotifyValidationErrors(request);
                return false;
            }

            var result = await _userService.CreatePasswordAsync(request);
            if (result)
            {
                await Bus.Publish(new PasswordCreatedEvent(request.Username));
                return true;
            }
            return false;
        }

        public async Task<bool> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
        {
            if (!request.IsValid())
            {
                NotifyValidationErrors(request);
                return false;
            }

            var result = await _userService.ChangePasswordAsync(request);
            if (result)
            {
                await Bus.Publish(new PasswordChangedEvent(request.Username));
                return true;
            }
            return false;
        }

        public async Task<bool> Handle(RemoveAccountCommand request, CancellationToken cancellationToken)
        {
            if (!request.IsValid())
            {
                NotifyValidationErrors(request);
                return false;
            }

            var result = await _userService.RemoveAccountAsync(request);
            if (result)
            {
                await Bus.Publish(new AccountRemovedEvent(request.Username));
                return true;
            }
            return false;
        }


        public async Task<bool> Handle(AdminUpdateUserCommand request, CancellationToken cancellationToken)
        {
            if (!request.IsValid())
            {
                NotifyValidationErrors(request);
                return false;
            }

            var user = await _userService.FindByNameAsync(request.Username);
            if (user == null)
            {
                await Bus.Publish(new DomainNotification("Username", "User not found"));
                return false;
            }
            await _userService.UpdateUserAsync(request);

            return true;
        }

        public async Task<bool> Handle(SaveUserClaimCommand request, CancellationToken cancellationToken)
        {
            if (!request.IsValid())
            {
                NotifyValidationErrors(request);
                return false;
            }

            var userDb = await _userService.FindByNameAsync(request.Username);
            if (userDb == null)
            {
                await Bus.Publish(new DomainNotification("Username", "User not found"));
                return false;
            }

            var claim = new Claim(request.Type, request.Value);

            var success = await _userService.SaveClaim(userDb.UserName, claim);

            if (success)
            {
                await Bus.Publish(new NewUserClaimEvent(request.Username, request.Type, request.Value));
                return true;
            }
            return false;
        }

        public async Task<bool> Handle(RemoveUserClaimCommand request, CancellationToken cancellationToken)
        {
            if (!request.IsValid())
            {
                NotifyValidationErrors(request);
                return false;
            }

            var userDb = await _userService.FindByNameAsync(request.Username);
            if (userDb == null)
            {
                await Bus.Publish(new DomainNotification("Username", "User not found"));
                return false;
            }

            var success = await _userService.RemoveClaim(userDb.UserName, request.Type, request.Value);

            if (success)
            {
                await Bus.Publish(new UserClaimRemovedEvent(request.Username, request.Type));
                return true;
            }
            return false;
        }

        public async Task<bool> Handle(RemoveUserRoleCommand request, CancellationToken cancellationToken)
        {
            if (!request.IsValid())
            {
                NotifyValidationErrors(request);
                return false;
            }

            var userDb = await _userService.FindByNameAsync(request.Username);
            if (userDb == null)
            {
                await Bus.Publish(new DomainNotification("Username", "User not found"));
                return false;
            }

            var success = await _userService.RemoveRole(userDb.UserName, request.Role);

            if (success)
            {
                await Bus.Publish(new UserRoleRemovedEvent(_user.Username, request.Role));
                return true;
            }
            return false;
        }

        public async Task<bool> Handle(SaveUserRoleCommand request, CancellationToken cancellationToken)
        {
            if (!request.IsValid())
            {
                NotifyValidationErrors(request);
                return false;
            }

            var user = await _userService.FindByNameAsync(request.Username);
            if (user == null)
            {
                await Bus.Publish(new DomainNotification("Username", "User not found"));
                return false;
            }

            var success = await _userService.SaveRole(user.UserName, request.Role);

            if (success)
            {
                await Bus.Publish(new UserRoleSavedEvent(_user.Username, request.Role));
                return true;
            }
            return false;
        }

        public async Task<bool> Handle(RemoveUserLoginCommand request, CancellationToken cancellationToken)
        {
            if (!request.IsValid())
            {
                NotifyValidationErrors(request);
                return false;
            }

            var user = await _userService.FindByNameAsync(request.Username);
            if (user == null)
            {
                await Bus.Publish(new DomainNotification("Username", "User not found"));
                return false;
            }

            var success = await _userService.RemoveLogin(user.UserName, request.LoginProvider, request.ProviderKey);

            if (success)
            {
                await Bus.Publish(new UserLoginRemovedEvent(_user.Username, request.LoginProvider, request.ProviderKey));
                return true;
            }
            return false;
        }

        public async Task<bool> Handle(AdminChangePasswordCommand request, CancellationToken cancellationToken)
        {
            if (!request.IsValid())
            {
                NotifyValidationErrors(request);
                return false;
            }

            var user = await _userService.FindByNameAsync(request.Username);
            if (user == null)
            {
                await Bus.Publish(new DomainNotification("Username", "User not found"));
                return false;
            }

            var success = await _userService.ResetPasswordAsync(request.Username, request.Password);

            if (success)
            {
                await Bus.Publish(new AdminChangedPasswordEvent(request.Username));
                return true;
            }
            return false;
        }


        public async Task<bool> Handle(SynchronizeClaimsCommand request, CancellationToken cancellationToken)
        {
            if (!request.IsValid())
            {
                NotifyValidationErrors(request);
                return false;
            }

            var userClaims = (await _userService.GetClaimByName(request.Username)).ToList();
            foreach (var claim in request.Claims)
            {
                var actualUserClaims = userClaims.Find(f => f.Type == claim.Type);
                if (actualUserClaims == null)
                {
                    await _userService.SaveClaim(request.Username, claim);
                }
                else
                {
                    var newValue = claim.Value;
                    var currentValue = actualUserClaims.Value;
                    if (currentValue != newValue)
                    {
                        await _userService.RemoveClaim(request.Username, actualUserClaims.Type, actualUserClaims.Value);
                        await _userService.SaveClaim(request.Username, claim);
                    }
                }
            }

            if (await Commit())
            {
                await Bus.Publish(new ClaimsSyncronizedEvent(request.Username, request.Claims));
                return true;
            }

            return false;
        }
    }
}
