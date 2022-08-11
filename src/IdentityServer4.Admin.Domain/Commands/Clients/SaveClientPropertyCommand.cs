using ByLearning.Admin.Domain.Validations.Client;

namespace ByLearning.Admin.Domain.Commands.Clients
{
    public class SaveClientPropertyCommand : ClientPropertyCommand
    {

        public SaveClientPropertyCommand(string clientId, string key, string value)
        {
            Key = key;
            ClientId = clientId;
            Value = value;
        }
        public override bool IsValid()
        {
            ValidationResult = new SaveClientPropertyCommandValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }
}