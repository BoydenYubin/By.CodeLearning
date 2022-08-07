using ByLearning.SSO.Domain.Validations.UserManagement;

namespace ByLearning.SSO.Domain.Commands.UserManagement
{
    public class RemoveAccountCommand : ProfileCommand
    {
        public RemoveAccountCommand(string username)
        {
            Username = username;
        }

        public override bool IsValid()
        {
            ValidationResult = new RemoveAccountCommandValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }
}