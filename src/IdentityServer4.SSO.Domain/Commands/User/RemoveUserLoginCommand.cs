using ByLearning.SSO.Domain.Validations.User;

namespace ByLearning.SSO.Domain.Commands.User
{
    public class RemoveUserLoginCommand : UserLoginCommand
    {
        public RemoveUserLoginCommand(string username, string loginProvider, string providerKey)
        {
            LoginProvider = loginProvider;
            ProviderKey = providerKey;
            Username = username;
        }
        public override bool IsValid()
        {
            ValidationResult = new RemoveUserLoginCommandValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }
}