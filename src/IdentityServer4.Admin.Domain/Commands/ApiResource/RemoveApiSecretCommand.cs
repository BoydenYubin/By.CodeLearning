using IdentityServer4.Models;
using ByLearning.Admin.Domain.Commands.Clients;
using ByLearning.Admin.Domain.Validations.ApiResource;

namespace ByLearning.Admin.Domain.Commands.ApiResource
{
    public class RemoveApiSecretCommand : ApiSecretCommand
    {

        public RemoveApiSecretCommand(string type, string value, string resourceName)
        {
            Type = type;
            Value = value;
            ResourceName = resourceName;
        }

        public override bool IsValid()
        {
            ValidationResult = new RemoveApiSecretCommandValidation().Validate(this);
            return ValidationResult.IsValid;
        }

        public Secret ToModel()
        {
            return new Secret(Value) { Type = Type };
        }
    }
}