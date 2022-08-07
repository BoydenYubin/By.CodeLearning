using FluentValidation;
using ByLearning.SSO.Domain.Commands.User;

namespace ByLearning.SSO.Domain.Validations.User
{
    public abstract class UserRoleValidation<T> : AbstractValidator<T> where T : UserRoleCommand
    {

        protected void ValidateUsername()
        {
            RuleFor(c => c.Username).NotEmpty().WithMessage("Username must be set");
        }

        protected void ValidateRole()
        {
            RuleFor(c => c.Role).NotEmpty().WithMessage("Role must be set");
        }
    }
}