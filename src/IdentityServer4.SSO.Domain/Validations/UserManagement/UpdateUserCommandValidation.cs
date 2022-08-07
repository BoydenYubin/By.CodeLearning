using ByLearning.SSO.Domain.Commands.UserManagement;

namespace ByLearning.SSO.Domain.Validations.UserManagement
{
    public class UpdateUserCommandValidation : UserManagementValidation<UserManagementCommand>
    {
        public UpdateUserCommandValidation()
        {
            ValidateEmail();
            ValidateName();
            ValidateUsername();
        }

    }
}