using IdentityServer4.Models;
using ByLearning.Admin.Domain.Validations.Client;

namespace ByLearning.Admin.Domain.Commands.Clients
{
    public class RemoveClientSecretCommand : ClientSecretCommand
    {
        public RemoveClientSecretCommand(string type, string value, string clientId)
        {
            Value = value;
            Type = type;
            ClientId = clientId;
        }


        public override bool IsValid()
        {
            ValidationResult = new RemoveClientSecretCommandValidation().Validate(this);
            return ValidationResult.IsValid;
        }

        public Secret ToModel()
        {
            return new Secret(Value) { Type = Type };
        }
    }
}