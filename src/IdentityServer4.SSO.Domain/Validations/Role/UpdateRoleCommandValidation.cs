using ByLearning.SSO.Domain.Commands.Role;

namespace ByLearning.SSO.Domain.Validations.Role
{
    public class UpdateRoleCommandValidation : RoleValidation<UpdateRoleCommand>
    {
        public UpdateRoleCommandValidation()
        {
            ValidateName();
            ValidateNewName();
        }
    }
}