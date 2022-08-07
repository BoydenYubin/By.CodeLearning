﻿using FluentValidation;
using ByLearning.SSO.Domain.Commands.UserManagement;

namespace ByLearning.SSO.Domain.Validations.UserManagement
{
    public class ProfileValidation<T> : AbstractValidator<T> where T : ProfileCommand
    {
        protected void ValidateUsername()
        {
            RuleFor(c => c.Username)
                .NotEmpty().WithMessage("Invalid user");
        }

        protected void ValidatePicture()
        {
            RuleFor(c => c.Picture)
                .NotEmpty().WithMessage("Please ensure you have entered the picture");
        }
    }
}
