using ByLearning.Admin.Domain.Validations.Client;

namespace ByLearning.Admin.Domain.Commands.Clients
{
    public class CopyClientCommand : ClientCommand
    {

        public CopyClientCommand(string clientId)
        {
            this.Client = new IdentityServer4.Models.Client() { ClientId = clientId };
        }

        public override bool IsValid()
        {
            ValidationResult = new CopyClientCommandValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }
}