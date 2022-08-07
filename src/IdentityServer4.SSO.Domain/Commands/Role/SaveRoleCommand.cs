using ByLearning.SSO.Domain.Validations.Role;

namespace ByLearning.SSO.Domain.Commands.Role
{
    public class SaveRoleCommand : RoleCommand
    {
        public SaveRoleCommand(string name)
        {
            Name = name;
        }

        public override bool IsValid()
        {
            ValidationResult = new SaveRoleCommandValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }
}