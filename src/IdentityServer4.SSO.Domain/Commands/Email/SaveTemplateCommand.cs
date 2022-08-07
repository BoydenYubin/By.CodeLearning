using ByLearning.SSO.Domain.Models;
using ByLearning.SSO.Domain.Validations.Email;

namespace ByLearning.SSO.Domain.Commands.Email
{
    public class SaveTemplateCommand : TemplateCommand
    {

        public SaveTemplateCommand(
            string subject,
            string content,
            string name,
            string userName)
        {
            UserName = userName;
            Subject = subject;
            Content = content;
            Name = name.Trim();
        }

        public override bool IsValid()
        {
            ValidationResult = new AddTemplateCommandValidation().Validate(this);
            return ValidationResult.IsValid;
        }

        public Template ToModel()
        {
            return new Template(Content, Subject, Name, UserName);
        }
    }
}