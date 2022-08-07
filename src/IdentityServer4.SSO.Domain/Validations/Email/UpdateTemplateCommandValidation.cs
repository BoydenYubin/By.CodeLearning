using ByLearning.SSO.Domain.Commands.Email;

namespace ByLearning.SSO.Domain.Validations.Email
{
    public class UpdateTemplateCommandValidation : TemplateValidation<UpdateTemplateCommand>
    {
        public UpdateTemplateCommandValidation()
        {
            ValidateOldName();
            ValidateName();
            ValidateSubject();
            ValidateContent();
        }
    }
}