using Microsoft.Extensions.Configuration;

namespace ByLearningORM.Util
{
    public class GetConfig
    {
        public static string GetConnectionString()
        {
            var config = new ConfigurationBuilder().AddJsonFile("Appsettings.json").Build();
            return config.GetValue<string>("ConnectionString");
        }
    }
}
