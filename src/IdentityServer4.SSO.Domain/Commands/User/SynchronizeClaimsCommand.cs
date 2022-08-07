using System.Collections.Generic;
using System.Security.Claims;
using ByLearning.SSO.Domain.Validations.User;

namespace ByLearning.SSO.Domain.Commands.User
{
    public class SynchronizeClaimsCommand : UserClaimCommand
    {

        public SynchronizeClaimsCommand(string username, IEnumerable<Claim> claims)
        {
            Claims = claims;
            Username = username;
        }
        public override bool IsValid()
        {
            ValidationResult = new SynchronizeClaimsCommandValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }
}