using ByLearning.SSO.Domain.Commands.User;

namespace ByLearning.SSO.Domain.Validations.User
{
    public class SendResetLinkCommandValidation : UserValidation<SendResetLinkCommand>
    {
        public SendResetLinkCommandValidation(SendResetLinkCommand sendResetLinkCommand)
        {
            ValidateUsernameOrEmail();
        }
    }
}