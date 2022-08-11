using ByLearning.Admin.Domain.Commands.ApiResource;

namespace ByLearning.Admin.Domain.Validations.ApiResource
{
    public class RemoveApiScopeCommandValidation : ApiScopeValidation<RemoveApiScopeCommand>
    {
        public RemoveApiScopeCommandValidation()
        {
            ValidateResourceName();
            ValidateScopeName();
        }
    }
}