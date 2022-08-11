using ByLearning.Admin.Domain.Validations.IdentityResource;

namespace ByLearning.Admin.Domain.Commands.IdentityResource
{
    public class UpdateIdentityResourceCommand : IdentityResourceCommand
    {
        public UpdateIdentityResourceCommand(IdentityServer4.Models.IdentityResource resource,string oldIdentityResourceName)
        {
            Resource = resource;
            this.OldIdentityResourceName = oldIdentityResourceName;
        }

        public override bool IsValid()
        {
            ValidationResult = new UpdateIdentityResourceCommandValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }
}
