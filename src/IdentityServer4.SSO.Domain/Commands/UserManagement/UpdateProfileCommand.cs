using ByLearning.SSO.Domain.Validations.UserManagement;
using System;

namespace ByLearning.SSO.Domain.Commands.UserManagement
{
    public class UpdateProfileCommand : ProfileCommand
    {

        public UpdateProfileCommand(string username, string url, string bio, string company, string jobTitle, string name, string phoneNumber, string socialNumber, DateTime? birthdate)
        {
            SocialNumber = socialNumber;
            Birthdate = birthdate;
            Username = username;
            Url = url;
            Bio = bio;
            Company = company;
            JobTitle = jobTitle;
            Name = name;
            PhoneNumber = phoneNumber;
        }


        public override bool IsValid()
        {
            ValidationResult = new UpdateProfileCommandValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }
}
