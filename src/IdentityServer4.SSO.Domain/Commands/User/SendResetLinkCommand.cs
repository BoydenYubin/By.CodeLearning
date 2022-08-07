using ByLearning.SSO.Domain.Validations.User;

namespace ByLearning.SSO.Domain.Commands.User
{
    public class SendResetLinkCommand : UserCommand
    {

        public SendResetLinkCommand(string emailOrUsername)
        {
            EmailOrUsername = emailOrUsername;
        }


        public override bool IsValid()
        {
            ValidationResult = new SendResetLinkCommandValidation(this).Validate(this);
            return ValidationResult.IsValid;
        }
    }
}