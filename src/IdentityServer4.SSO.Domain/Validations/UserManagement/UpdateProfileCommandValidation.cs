using ByLearning.SSO.Domain.Commands.UserManagement;

namespace ByLearning.SSO.Domain.Validations.UserManagement
{
    public class UpdateProfileCommandValidation : ProfileValidation<ProfileCommand>
    {
        public UpdateProfileCommandValidation()
        {
            ValidateUsername();
        }
    }
}
