using ByLearning.Admin.Domain.Commands.Clients;

namespace ByLearning.Admin.Domain.Validations.Client
{
    public class RemoveClientSecretCommandValidation : ClientSecretValidation<ClientSecretCommand>
    {
        public RemoveClientSecretCommandValidation()
        {
            ValidateClientId();
            ValidateType();
            ValidateValue();
        }
    }
}