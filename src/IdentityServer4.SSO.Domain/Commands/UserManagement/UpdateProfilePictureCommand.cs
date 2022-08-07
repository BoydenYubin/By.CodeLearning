using ByLearning.SSO.Domain.Validations.UserManagement;

namespace ByLearning.SSO.Domain.Commands.UserManagement
{
    public class UpdateProfilePictureCommand : ProfileCommand
    {
        public UpdateProfilePictureCommand(string username, string picture)
        {
            Username = username;
            Picture = picture;
        }

        public override bool IsValid()
        {
            ValidationResult = new UpdateProfilePictureCommandValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }
}