using ByLearning.SSO.Domain.Commands.User;

namespace ByLearning.SSO.Domain.Validations.User
{
    public class ConfirmEmailCommandValidation : UserValidation<ConfirmEmailCommand>
    {
        public ConfirmEmailCommandValidation()
        {
            ValidateEmail();
            ValidateCode();
        }
    }
}