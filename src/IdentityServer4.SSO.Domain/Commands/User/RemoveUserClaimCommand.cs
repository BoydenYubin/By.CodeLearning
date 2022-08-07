using ByLearning.SSO.Domain.Validations.User;

namespace ByLearning.SSO.Domain.Commands.User
{
    public class RemoveUserClaimCommand : UserClaimCommand
    {

        public RemoveUserClaimCommand(string username, string type, string value)
        {
            Value = value;
            Type = type;
            Username = username;
        }
        public override bool IsValid()
        {
            ValidationResult = new RemoveUserClaimCommandValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }
}