using ByLearning.Admin.Domain.Validations.PersistedGrant;

namespace ByLearning.Admin.Domain.Commands.PersistedGrant
{
    public class UpdatePersistedGrantCommand : PersistedGrantCommand
    {
        public UpdatePersistedGrantCommand()
        {
           
        }

        public override bool IsValid()
        {
            ValidationResult = new UpdatePersistedGrantCommandValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }
}