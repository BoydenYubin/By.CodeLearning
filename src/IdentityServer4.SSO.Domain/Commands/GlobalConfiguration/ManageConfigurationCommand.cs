using ByLearning.SSO.Domain.Models;
using ByLearning.SSO.Domain.Validations.GlobalConfiguration;

namespace ByLearning.SSO.Domain.Commands.GlobalConfiguration
{
    public class ManageConfigurationCommand : GlobalConfigurationCommand
    {
        public ManageConfigurationCommand(string key, string value, bool sensitive, bool isPublic)
        {
            Key = key;
            Value = value;
            Sensitive = sensitive;
            IsPublic = isPublic;
        }
        public override bool IsValid()
        {
            ValidationResult = new CreateConfigurationValidation().Validate(this);
            return ValidationResult.IsValid;
        }

        public GlobalConfigurationSettings ToEntity()
        {
            return new GlobalConfigurationSettings(Key, Value, Sensitive, IsPublic);
        }
    }
}
