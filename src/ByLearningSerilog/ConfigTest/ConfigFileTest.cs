using Microsoft.Extensions.Configuration;
using Serilog;
using System.IO;
using Xunit;

namespace ByLearningSerilog.ConfigTest
{
    public class ConfigFileShouldWorkTest
    {
        [Fact]
        public void HowToUseConfigFileTest()
        {
            var configuration = new ConfigurationBuilder()
                                   .SetBasePath(Directory.GetCurrentDirectory())
                                   .AddJsonFile("ConfigTest//appsettings.json", false, true)
                                   .Build();
            var logger = new LoggerConfiguration()
                            .ReadFrom.Configuration(configuration)
                            .CreateLogger();

            logger.Information("Hello, world!");
        }
    }
}
