using FluentValidation;
using ByLearning.SSO.Domain.Commands.User;

namespace ByLearning.SSO.Domain.Validations.User
{
    public abstract class UserLoginValidation<T> : AbstractValidator<T> where T : UserLoginCommand
    {

        protected void ValidateUsername()
        {
            RuleFor(c => c.Username).NotEmpty().WithMessage("Username must be set");
        }

        protected void ValidateLoginProvider()
        {
            RuleFor(c => c.LoginProvider).NotEmpty().WithMessage("Login Provider must be set");
        }
        protected void ValidateProviderKey()
        {
            RuleFor(c => c.ProviderKey).NotEmpty().WithMessage("Provider Key must be set");
        }
    }
}