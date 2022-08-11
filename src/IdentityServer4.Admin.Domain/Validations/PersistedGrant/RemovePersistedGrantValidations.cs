using ByLearning.Admin.Domain.Commands.PersistedGrant;

namespace ByLearning.Admin.Domain.Validations.PersistedGrant
{
    public class RemovePersistedGrantCommandValidation : PersistedGrantValidation<RemovePersistedGrantCommand>
    {
        public RemovePersistedGrantCommandValidation()
        {
            this.ValidateKey();
        }
    }
}