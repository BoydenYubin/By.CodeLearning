using ByLearning.Admin.Domain.Commands.IdentityResource;

namespace ByLearning.Admin.Domain.Validations.IdentityResource
{
    public class RemoveIdentityResourceCommandValidation : IdentityResourceValidation<RemoveIdentityResourceCommand>
    {
        public RemoveIdentityResourceCommandValidation()
        {
            ValidateName();
        }
    }
}