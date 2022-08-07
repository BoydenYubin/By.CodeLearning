using ByLearning.SSO.Domain.Validations.Email;

namespace ByLearning.SSO.Domain.Commands.Email
{
    public class RemoveTemplateCommand : TemplateCommand
    {

        public RemoveTemplateCommand(string name)
        {
            Name = name.Trim();
        }

        public override bool IsValid()
        {
            ValidationResult = new RemoveTemplateCommandValidation().Validate(this);
            return ValidationResult.IsValid;
        }

    }
}