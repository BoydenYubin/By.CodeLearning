using ByLearning.Admin.Domain.Commands.Clients;

namespace ByLearning.Admin.Domain.Validations.Client
{
    public class SaveClientClaimCommandValidation : ClientClaimValidation<SaveClientClaimCommand>
    {
        public SaveClientClaimCommandValidation()
        {
            ValidateClientId();
            ValidateKey();
            ValidateValue();
        }
    }
}