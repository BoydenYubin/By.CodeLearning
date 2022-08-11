using ByLearning.Admin.Domain.Commands.Clients;

namespace ByLearning.Admin.Domain.Validations.Client
{
    public class SaveClientCommandValidation : ClientValidation<SaveClientCommand>
    {
        public SaveClientCommandValidation()
        {
            ValidateClientId();
            ValidateClientName();
            ValidatePostLogoutTrailingSlash();
            ValidateClientUriTrailingSlash();
        }
    }
}