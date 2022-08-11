using ByLearning.Admin.Domain.Commands.ApiResource;
using ByLearning.Admin.Domain.Validations.Client;

namespace ByLearning.Admin.Domain.Validations.ApiResource
{
    public class SaveApiSecretCommandValidation : ApiSecretValidation<SaveApiSecretCommand>
    {
        public SaveApiSecretCommandValidation()
        {
            ValidateResourceName();
            ValidateType();
            ValidateValue();
            ValidateHashType();
        }
    }
}