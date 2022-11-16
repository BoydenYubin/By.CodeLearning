using ByLearning.SagaTransitionConfiguration.Model;
using Microsoft.Extensions.Configuration;

namespace ByLearning.SagaTransitionConfiguration
{
    public class GlobalConfiguration
    {
        private static IConfiguration Configuration
            => new ConfigurationBuilder()
                  .AddYamlFile("global_setting.yaml", false, true)
                  .Build();

        public static GlobalSettings GlobalSettings => Configuration.GetSection("GlobalSettings").Get<GlobalSettings>();
    }
}
