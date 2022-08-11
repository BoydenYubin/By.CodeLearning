using ByLearning.Admin.Domain.Commands.Clients;

namespace ByLearning.Admin.Domain.Validations.Client
{
    public class RemoveClientClaimCommandValidation : ClientClaimValidation<RemoveClientClaimCommand>
    {
        public RemoveClientClaimCommandValidation()
        {
            ValidateClientId();
            ValidateKey();
        }
    }
}