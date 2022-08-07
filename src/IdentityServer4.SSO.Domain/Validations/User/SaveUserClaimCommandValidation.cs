using ByLearning.SSO.Domain.Commands.User;

namespace ByLearning.SSO.Domain.Validations.User
{
    public class SaveUserClaimCommandValidation : UserClaimValidation<SaveUserClaimCommand>
    {
        public SaveUserClaimCommandValidation()
        {
            ValidateUsername();
            ValidateKey();
            ValidateValue();
        }
    }
}