using ByLearning.Admin.Domain.Commands.Clients;

namespace ByLearning.Admin.Domain.Validations.Client
{
    public class CopyClientCommandValidation : ClientValidation<CopyClientCommand>
    {
        public CopyClientCommandValidation()
        {
            ValidateClientId();
        }
    }
}