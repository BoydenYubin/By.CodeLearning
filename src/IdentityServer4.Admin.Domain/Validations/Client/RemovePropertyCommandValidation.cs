using ByLearning.Admin.Domain.Commands.Clients;

namespace ByLearning.Admin.Domain.Validations.Client
{
    public class RemovePropertyCommandValidation : ClientPropertyValidation<RemovePropertyCommand>
    {
        public RemovePropertyCommandValidation()
        {
            ValidateClientId();
            ValidateKey();
        }
    }
}