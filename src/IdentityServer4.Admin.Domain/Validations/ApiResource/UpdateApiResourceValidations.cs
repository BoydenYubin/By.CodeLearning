using ByLearning.Admin.Domain.Commands.ApiResource;

namespace ByLearning.Admin.Domain.Validations.ApiResource
{
    public class UpdateApiResourceCommandValidation : ApiResourceValidation<UpdateApiResourceCommand>
    {
        public UpdateApiResourceCommandValidation()
        {
            ValidateName();
        }
    }
}