using ByLearning.Admin.Domain.Commands.Clients;

namespace ByLearning.Admin.Domain.Validations.Client
{
    public class SaveClientPropertyCommandValidation : ClientPropertyValidation<SaveClientPropertyCommand>
    {
        public SaveClientPropertyCommandValidation()
        {
            ValidateClientId();
            ValidateKey();
            ValidateValue();
        }
    }
}