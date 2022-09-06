using Microsoft.Extensions.Configuration;

namespace DemoConsulConfig
{
    public static class DemoConsulConfigSettings
    {
        /// <summary>
        /// Please set up the consul address on json file
        /// then get the consul address
        /// </summary>
        public static string ConsulAddress
        {
            get
            {
                var config = new ConfigurationBuilder().AddJsonFile("service.config.json").Build();
                return config["ConsulAddress"];
            }
        }
    }
}
