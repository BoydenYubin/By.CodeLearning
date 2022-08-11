using ByLearning.Admin.Domain.Commands.Clients;

namespace ByLearning.Admin.Domain.Validations.Client
{
    public class SaveClientSecretCommandValidation : ClientSecretValidation<SaveClientSecretCommand>
    {
        public SaveClientSecretCommandValidation()
        {
            ValidateClientId();
            ValidateType();
            ValidateValue();
            ValidateHashType();

        }
    }
}