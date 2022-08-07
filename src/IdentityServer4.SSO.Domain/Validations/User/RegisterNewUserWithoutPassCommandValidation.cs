using ByLearning.SSO.Domain.Commands.User;

namespace ByLearning.SSO.Domain.Validations.User
{
    public class RegisterNewUserWithoutPassCommandValidation : UserValidation<UserCommand>
    {
        public RegisterNewUserWithoutPassCommandValidation()
        {
            ValidateName();
            ValidateUsername();
            ValidateEmail();
            ValidateProvider();
            ValidateProviderId();
        }



    }
}