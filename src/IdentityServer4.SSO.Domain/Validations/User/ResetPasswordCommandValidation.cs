using FluentValidation;
using ByLearning.SSO.Domain.Commands.User;

namespace ByLearning.SSO.Domain.Validations.User
{
    public class ResetPasswordCommandValidation : UserValidation<ResetPasswordCommand>
    {
        public ResetPasswordCommandValidation()
        {
            ValidateEmail();
            ValidatePassword();
            ValidateCode();
        }

        protected void ValidateCode()
        {
            RuleFor(c => c.Code)
                .NotEmpty();
        }

    }
}