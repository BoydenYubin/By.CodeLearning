using ByLearning.SSO.Domain.Commands.User;

namespace ByLearning.SSO.Domain.Validations.User
{
    public class RegisterNewUserCommandValidation : UserValidation<UserCommand>
    {
        public RegisterNewUserCommandValidation()
        {
            ValidateName();
            ValidateUsername();
            ValidateEmail();
            ValidatePassword();

        }
    }
}
