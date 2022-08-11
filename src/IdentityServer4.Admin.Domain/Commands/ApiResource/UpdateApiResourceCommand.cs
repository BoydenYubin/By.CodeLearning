using ByLearning.Admin.Domain.Validations.ApiResource;

namespace ByLearning.Admin.Domain.Commands.ApiResource
{
    public class UpdateApiResourceCommand : ApiResourceCommand
    {

        public UpdateApiResourceCommand(IdentityServer4.Models.ApiResource resource, string oldResourceName)
        {
            OldResourceName = oldResourceName;
            Resource = resource;
        }

        public override bool IsValid()
        {
            ValidationResult = new UpdateApiResourceCommandValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }
}