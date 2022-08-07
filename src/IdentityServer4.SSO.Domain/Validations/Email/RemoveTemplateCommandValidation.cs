using ByLearning.SSO.Domain.Commands.Email;

namespace ByLearning.SSO.Domain.Validations.Email
{
    public class RemoveTemplateCommandValidation : TemplateValidation<RemoveTemplateCommand>
    {
        public RemoveTemplateCommandValidation()
        {
            ValidateName();
        }
    }
}