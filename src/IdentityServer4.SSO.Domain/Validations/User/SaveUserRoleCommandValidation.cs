using ByLearning.SSO.Domain.Commands.User;

namespace ByLearning.SSO.Domain.Validations.User
{
    public class SaveUserRoleCommandValidation : UserRoleValidation<SaveUserRoleCommand>
    {
        public SaveUserRoleCommandValidation()
        {
            ValidateUsername();
            ValidateRole();
        }
    }
}