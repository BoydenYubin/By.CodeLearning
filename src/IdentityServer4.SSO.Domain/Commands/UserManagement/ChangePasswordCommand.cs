﻿using ByLearning.SSO.Domain.Validations.UserManagement;

namespace ByLearning.SSO.Domain.Commands.UserManagement
{
    public class ChangePasswordCommand : PasswordCommand
    {

        public ChangePasswordCommand(string username, string oldPassword, string newPassword, string confirmPassword)
        {
            Username = username;
            OldPassword = oldPassword;
            Password = newPassword;
            ConfirmPassword = confirmPassword;
        }

        public override bool IsValid()
        {
            ValidationResult = new ChangePasswordCommandValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }
}