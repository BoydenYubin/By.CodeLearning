using ByLearning.Admin.Domain.Commands.ApiResource;
using ByLearning.Admin.Domain.Validations.Client;

namespace ByLearning.Admin.Domain.Validations.ApiResource
{
    public class RemoveApiSecretCommandValidation : ApiSecretValidation<RemoveApiSecretCommand>
    {
        public RemoveApiSecretCommandValidation()
        {
            ValidateResourceName();
            ValidateType();
            ValidateValue();
        }
    }
}