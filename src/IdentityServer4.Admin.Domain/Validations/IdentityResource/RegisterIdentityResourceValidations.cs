using ByLearning.Admin.Domain.Commands.IdentityResource;

namespace ByLearning.Admin.Domain.Validations.IdentityResource
{
    public class RegisterIdentityResourceCommandValidation : IdentityResourceValidation<RegisterIdentityResourceCommand>
    {
        public RegisterIdentityResourceCommandValidation()
        {
            ValidateName();
        }
    }
}