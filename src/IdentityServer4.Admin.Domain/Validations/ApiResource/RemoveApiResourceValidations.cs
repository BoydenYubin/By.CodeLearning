using ByLearning.Admin.Domain.Commands.ApiResource;

namespace ByLearning.Admin.Domain.Validations.ApiResource
{
    public class RemoveApiResourceCommandValidation : ApiResourceValidation<RemoveApiResourceCommand>
    {
        public RemoveApiResourceCommandValidation()
        {
            ValidateName();
        }
    }
}