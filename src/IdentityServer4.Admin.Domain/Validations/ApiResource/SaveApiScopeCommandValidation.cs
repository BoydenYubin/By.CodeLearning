using ByLearning.Admin.Domain.Commands.ApiResource;

namespace ByLearning.Admin.Domain.Validations.ApiResource
{
    public class SaveApiScopeCommandValidation : ApiScopeValidation<SaveApiScopeCommand>
    {
        public SaveApiScopeCommandValidation()
        {
            ValidateResourceName();
        }
    }
}