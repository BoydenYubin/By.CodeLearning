using System;
using FluentValidation;
using ByLearning.Admin.Domain.Commands.IdentityResource;

namespace ByLearning.Admin.Domain.Validations.IdentityResource
{
    public abstract class IdentityResourceValidation<T> : AbstractValidator<T> where T : IdentityResourceCommand
    {
        protected void ValidateName()
        {
            RuleFor(c => c.Resource.Name).NotEmpty().WithMessage("Invalid resource name");
        }

    }
}