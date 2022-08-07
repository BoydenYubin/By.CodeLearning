using ByLearning.SSO.Domain.Commands.User;

namespace ByLearning.SSO.Domain.Validations.User
{
    public class RemoveUserRoleCommandValidation : UserRoleValidation<RemoveUserRoleCommand>
    {
        public RemoveUserRoleCommandValidation()
        {
            ValidateUsername();
            ValidateRole();
        }
    }
}