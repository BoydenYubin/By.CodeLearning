using ByLearning.Admin.Domain.Validations.IdentityResource;

namespace ByLearning.Admin.Domain.Commands.IdentityResource
{
    public class RemoveIdentityResourceCommand : IdentityResourceCommand
    {

        public RemoveIdentityResourceCommand(string name)
        {
            Resource = new IdentityServer4.Models.IdentityResource() { Name = name };
        }

        public override bool IsValid()
        {
            ValidationResult = new RemoveIdentityResourceCommandValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }
}