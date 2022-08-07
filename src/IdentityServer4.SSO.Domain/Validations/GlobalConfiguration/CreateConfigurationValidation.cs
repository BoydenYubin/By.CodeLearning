using ByLearning.SSO.Domain.Commands.GlobalConfiguration;

namespace ByLearning.SSO.Domain.Validations.GlobalConfiguration
{
    public class CreateConfigurationValidation : GlobalConfigurationValidation<ManageConfigurationCommand>
    {
        public CreateConfigurationValidation()
        {
            ValidateValue();
            ValidateKey();
        }
    }
}
