using System;
using FluentValidation;
using ByLearning.Admin.Domain.Commands.PersistedGrant;

namespace ByLearning.Admin.Domain.Validations.PersistedGrant
{
    public abstract class PersistedGrantValidation<T> : AbstractValidator<T> where T : PersistedGrantCommand
    {
        protected void ValidateKey()
        {
            RuleFor(c => c.Key).NotEmpty();
        }
    }
}