using ByLearning.Admin.Domain.Commands.ApiResource;

namespace ByLearning.Admin.Domain.Validations.ApiResource
{
    public class RemoveApiCommandValidation : ApiSecretValidation<RemoveApiSecretCommand>
    {
        public RemoveApiCommandValidation()
        {
            ValidateResourceName();
        }
    }
}