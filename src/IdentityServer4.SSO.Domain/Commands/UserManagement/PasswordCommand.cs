using ByLearning.Domain.Core.Commands;

namespace ByLearning.SSO.Domain.Commands.UserManagement
{
    public abstract class PasswordCommand : Command
    {
        public string Username { get; protected set; }
        public string Password { get; protected set; }
        public string ConfirmPassword { get; protected set; }
        public string OldPassword { get; protected set; }
    }
}