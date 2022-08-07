using ByLearning.SSO.Domain.Commands.Role;

namespace ByLearning.SSO.Domain.Validations.Role
{
    public class SaveRoleCommandValidation : RoleValidation<SaveRoleCommand>
    {
        public SaveRoleCommandValidation()
        {
            ValidateName();
        }
    }
}