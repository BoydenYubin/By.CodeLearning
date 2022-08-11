using ByLearning.Admin.Domain.Commands.IdentityResource;

namespace ByLearning.Admin.Domain.Validations.IdentityResource
{
    public class UpdateIdentityResourceCommandValidation : IdentityResourceValidation<UpdateIdentityResourceCommand>
    {
        public UpdateIdentityResourceCommandValidation()
        {
            ValidateName();
        }
    }
}