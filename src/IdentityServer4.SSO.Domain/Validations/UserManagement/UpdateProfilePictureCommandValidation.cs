using ByLearning.SSO.Domain.Commands.UserManagement;

namespace ByLearning.SSO.Domain.Validations.UserManagement
{
    public class UpdateProfilePictureCommandValidation : ProfileValidation<UpdateProfilePictureCommand>
    {
        public UpdateProfilePictureCommandValidation()
        {
            ValidatePicture();
        }
    }
}