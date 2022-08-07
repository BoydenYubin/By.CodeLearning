using ByLearning.SSO.Domain.Commands.User;

namespace ByLearning.SSO.Domain.Validations.User
{
    public class RegisterNewUserWithProviderCommandValidation : UserValidation<UserCommand>
    {
        public RegisterNewUserWithProviderCommandValidation()
        {
            ValidateName();
            ValidateUsername();
            ValidateEmail();
            ValidateProvider();
            ValidateProviderId();
        }
    }
}