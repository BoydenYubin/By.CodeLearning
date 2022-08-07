using ByLearning.SSO.Domain.Commands.Role;

namespace ByLearning.SSO.Domain.Validations.Role
{
    public class RemoveUserFromRoleValidation : RoleValidation<RemoveUserFromRoleCommand>
    {
        public RemoveUserFromRoleValidation()
        {
            ValidateName();
            ValidateUsername();
        }
    }
}