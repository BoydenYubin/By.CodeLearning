using ByLearning.SSO.Domain.Validations.User;

namespace ByLearning.SSO.Domain.Commands.User
{
    public class RemoveUserRoleCommand : UserRoleCommand
    {

        public RemoveUserRoleCommand(string username, string role)
        {
            Role = role;
            Username = username;
        }
        public override bool IsValid()
        {
            ValidationResult = new RemoveUserRoleCommandValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }
}