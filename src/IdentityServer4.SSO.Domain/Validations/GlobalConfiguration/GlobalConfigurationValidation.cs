using FluentValidation;
using ByLearning.SSO.Domain.Commands.GlobalConfiguration;

namespace ByLearning.SSO.Domain.Validations.GlobalConfiguration
{
    public abstract class GlobalConfigurationValidation<T> : AbstractValidator<T> where T : GlobalConfigurationCommand
    {
        protected void ValidateKey()
        {
            RuleFor(r => r.Key).NotEmpty();
        }

        protected void ValidateValue()
        {
            RuleFor(r => r.Key).NotEmpty();
        }
    }
}
