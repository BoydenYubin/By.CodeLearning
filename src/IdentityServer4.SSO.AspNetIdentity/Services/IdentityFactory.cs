using ByLearning.SSO.AspNetIdentity.Models.Identity;
using ByLearning.SSO.Domain.Commands.User;
using ByLearning.SSO.Domain.Commands.UserManagement;
using ByLearning.SSO.Domain.Interfaces;

namespace ByLearning.SSO.AspNetIdentity.Services
{
    public class IdentityFactory : IIdentityFactory<UserIdentity>, IRoleFactory<RoleIdentity>
    {
        public UserIdentity Create(UserCommand user)
        {
            return new UserIdentity
            {
                PhoneNumber = user.PhoneNumber,
                Email = user.Email,
                UserName = user.Username,
                EmailConfirmed = user.EmailConfirmed,
                LockoutEnd = null,
            };
        }

        public RoleIdentity CreateRole(string name)
        {
            return new RoleIdentity(name);
        }


        public void UpdateInfo(AdminUpdateUserCommand command, UserIdentity userDb)
        {
            userDb.Email = command.Email;
            userDb.EmailConfirmed = command.EmailConfirmed;
            userDb.AccessFailedCount = command.AccessFailedCount;
            userDb.LockoutEnabled = command.LockoutEnabled;
            userDb.LockoutEnd = command.LockoutEnd;
            userDb.TwoFactorEnabled = command.TwoFactorEnabled;
            userDb.PhoneNumber = command.PhoneNumber;
            userDb.PhoneNumberConfirmed = command.PhoneNumberConfirmed;
        }

        public void UpdateProfile(UpdateProfileCommand command, UserIdentity user)
        {
            user.PhoneNumber = command.PhoneNumber;
        }

    }
}
