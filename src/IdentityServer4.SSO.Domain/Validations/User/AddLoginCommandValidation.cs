using ByLearning.SSO.Domain.Commands.User;

namespace ByLearning.SSO.Domain.Validations.User
{
    public class AddLoginCommandValidation : UserValidation<AddLoginCommand>
    {
        public AddLoginCommandValidation()
        {
            ValidateEmail();
            ValidateProvider();
            ValidateProviderId();
        }

    }
}