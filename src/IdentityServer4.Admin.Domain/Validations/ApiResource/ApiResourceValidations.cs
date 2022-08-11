using System;
using FluentValidation;
using ByLearning.Admin.Domain.Commands.ApiResource;

namespace ByLearning.Admin.Domain.Validations.ApiResource
{
    public abstract class ApiResourceValidation<T> : AbstractValidator<T> where T : ApiResourceCommand
    {
        protected void ValidateName()
        {
            RuleFor(c => c.Resource.Name).NotEmpty().WithMessage("Invalid resource name");
        }
    }
}