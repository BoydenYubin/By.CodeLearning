using ByLearning.Admin.Domain.Commands.ApiResource;

namespace ByLearning.Admin.Domain.Validations.ApiResource
{
    public class RegisterApiResourceCommandValidation : ApiResourceValidation<RegisterApiResourceCommand>
    {
        public RegisterApiResourceCommandValidation()
        {
            ValidateName();
        }
    }
}