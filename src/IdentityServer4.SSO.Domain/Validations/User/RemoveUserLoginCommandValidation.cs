using ByLearning.SSO.Domain.Commands.User;

namespace ByLearning.SSO.Domain.Validations.User
{
    public class RemoveUserLoginCommandValidation : UserLoginValidation<RemoveUserLoginCommand>
    {
        public RemoveUserLoginCommandValidation()
        {
            ValidateUsername();
            ValidateLoginProvider();
            ValidateProviderKey();
        }
    }
}