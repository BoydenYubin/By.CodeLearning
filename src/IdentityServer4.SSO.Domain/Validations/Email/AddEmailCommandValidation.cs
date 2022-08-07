using ByLearning.SSO.Domain.Commands.Email;

namespace ByLearning.SSO.Domain.Validations.Email
{
    public class AddTemplateCommandValidation : TemplateValidation<SaveTemplateCommand>
    {
        public AddTemplateCommandValidation()
        {
            ValidateName();
            ValidateContent();
            
        }
    }
}