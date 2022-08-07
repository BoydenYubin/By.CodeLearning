using ByLearning.SSO.Domain.Commands.User;

namespace ByLearning.SSO.Domain.Validations.User
{
    public class SynchronizeClaimsCommandValidation : UserClaimValidation<SynchronizeClaimsCommand>
    {
        public SynchronizeClaimsCommandValidation()
        {
            ValidateUsername();
            ValidateClaims();
        }
    }
}