using ByLearning.SSO.Domain.Commands.User;

namespace ByLearning.SSO.Domain.Validations.User
{
    public class AdminChangePasswordCommandValidation : UserValidation<AdminChangePasswordCommand>
    {
        public AdminChangePasswordCommandValidation()
        {
            ValidateUsername();
        }
    }
}