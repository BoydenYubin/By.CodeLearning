using FluentValidation.Results;
using IdentityServer4.Models;
using ByLearning.Admin.Domain.Validations.ApiResource;

namespace ByLearning.Admin.Domain.Commands.ApiResource
{
    public class RegisterApiResourceCommand : ApiResourceCommand
    {

        public RegisterApiResourceCommand(IdentityServer4.Models.ApiResource apiResource)
        {
            Resource = apiResource;
        }

        public override bool IsValid()
        {
            ValidationResult = new RegisterApiResourceCommandValidation().Validate(this);
            return ValidationResult.IsValid;
        }

        public IdentityServer4.Models.ApiResource ToModel()
        {
            Resource.Scopes.Add(new Scope(Resource.Name, Resource.DisplayName));
            return Resource;
        }
    }
}