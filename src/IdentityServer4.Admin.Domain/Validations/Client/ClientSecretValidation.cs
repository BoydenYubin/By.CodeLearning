using FluentValidation;
using ByLearning.Admin.Domain.Commands.Clients;

namespace ByLearning.Admin.Domain.Validations.Client
{
    public abstract class ClientSecretValidation<T> : AbstractValidator<T> where T : ClientSecretCommand
    {

        protected void ValidateClientId()
        {
            RuleFor(c => c.ClientId).NotEmpty().WithMessage("ClientId must be set");
        }

        protected void ValidateValue()
        {
            RuleFor(c => c.Value).NotEmpty().WithMessage("Secret must be set");
        }
        protected void ValidateType()
        {
            RuleFor(c => c.Type).NotEmpty().WithMessage("Please ensure you have selected Type");
        }

        protected void ValidateHashType()
        {
            RuleFor(c => c.Hash).InclusiveBetween(0, 1).WithMessage("Please set hash type");
        }
    }

}