using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace ByLearningConsul.ServiceRegistration
{
    public static class ConsulServiceRegistration
    {
        private static void ThrowNullException(IServiceCollection service)
        {
            if (service == null)
            {
                throw new NullReferenceException("service cann't be null!");
            }
        }
        public static IServiceCollection AddConsul(this IServiceCollection service)
        {
            ThrowNullException(service);
            return service.AddConsul(config: null);
        }
        public static IServiceCollection AddConsul(this IServiceCollection service,string configPath)
        {
            ThrowNullException(service);
            if (string.IsNullOrEmpty(configPath))
            {
                throw new NotSupportedException("Config path should be correct");
            }
            var config = new ConfigurationBuilder().AddJsonFile(configPath).Build();
            return service.AddConsul(config);
        }
        public static IServiceCollection AddConsul(this IServiceCollection service, IConfiguration config)
        {
            ThrowNullException(service);
            if (config == null)
            {
                throw new NullReferenceException("Config can't be null!");
            }
            service.Configure<ConsulServiceOptions>(config);     
            return service;
        }
    }
}
