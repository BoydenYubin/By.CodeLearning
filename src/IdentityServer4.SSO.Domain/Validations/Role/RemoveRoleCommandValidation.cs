using ByLearning.SSO.Domain.Commands.Role;

namespace ByLearning.SSO.Domain.Validations.Role
{
    public class RemoveRoleCommandValidation : RoleValidation<RemoveRoleCommand>
    {
        public RemoveRoleCommandValidation()
        {
            ValidateName();
        }
    }
}
