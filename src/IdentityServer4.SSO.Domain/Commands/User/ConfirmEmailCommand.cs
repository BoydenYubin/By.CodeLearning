using ByLearning.SSO.Domain.Validations.User;

namespace ByLearning.SSO.Domain.Commands.User
{
    public class ConfirmEmailCommand : UserCommand
    {

        public ConfirmEmailCommand(string code, string email)
        {
            Code = code;
            Email = email;
        }

        public override bool IsValid()
        {
            ValidationResult = new ConfirmEmailCommandValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }
}