using ByLearning.SSO.Domain.Commands.UserManagement;

namespace ByLearning.SSO.Domain.Validations.UserManagement
{
    public class RemoveAccountCommandValidation : ProfileValidation<RemoveAccountCommand>
    {
        public RemoveAccountCommandValidation()
        {
            ValidateUsername();
        }
    }
}