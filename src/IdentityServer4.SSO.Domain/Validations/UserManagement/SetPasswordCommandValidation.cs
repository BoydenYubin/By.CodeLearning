    using ByLearning.SSO.Domain.Commands.UserManagement;

    namespace ByLearning.SSO.Domain.Validations.UserManagement
{
    public class SetPasswordCommandValidation : PasswordCommandValidation<SetPasswordCommand>
    {
        public SetPasswordCommandValidation()
        {
            ValidateUsername();
            ValidatePassword();
        }
    }
}