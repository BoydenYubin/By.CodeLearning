using ByLearning.SagaTransitionConfiguration;
using Shouldly;
using Xunit;

namespace ByLearning.SagaTransitionTest
{
    public class ConfigurationTest
    {
        [Fact]
        public void ConfigurationShouldGetCorrectValueTest()
        {
            GlobalConfiguration.GlobalSettings.RabbitMqConfiguration.Broker_Address.ShouldBe("test");
        }
    }
}
