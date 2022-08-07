using ByLearning.SSO.Domain.Commands.User;

namespace ByLearning.SSO.Domain.Validations.User
{
    public class RemoveUserClaimCommandValidation : UserClaimValidation<RemoveUserClaimCommand>
    {
        public RemoveUserClaimCommandValidation()
        {
            ValidateUsername();
            ValidateKey();
        }
    }
}