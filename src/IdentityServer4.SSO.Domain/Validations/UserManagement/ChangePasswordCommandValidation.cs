using ByLearning.SSO.Domain.Commands.UserManagement;

namespace ByLearning.SSO.Domain.Validations.UserManagement
{
    public class ChangePasswordCommandValidation : PasswordCommandValidation<ChangePasswordCommand>
    {
        public ChangePasswordCommandValidation()
        {
            ValidateUsername();
            ValidateOldPassword();
            ValidatePassword();
        }
    }
}