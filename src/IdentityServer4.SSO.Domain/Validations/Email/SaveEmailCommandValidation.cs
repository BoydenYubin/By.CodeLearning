using ByLearning.SSO.Domain.Commands.Email;

namespace ByLearning.SSO.Domain.Validations.Email
{
    public class SaveEmailCommandValidation : EmailValidation<SaveEmailCommand>
    {
        public SaveEmailCommandValidation()
        {
            ValidateSubject();
            ValidateSubject();
            ValidateSendAddress();
            ValidateSendName();
        }
    }
}