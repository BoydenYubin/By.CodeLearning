using Consul;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using System;
using System.Linq;

namespace ByLearningConsul.ServiceRegistration
{
    public static class ConsulServiceApplicationBuilderExtension
    {
        public static IApplicationBuilder UseConsul(this IApplicationBuilder app)
        {
            var lifetime = app.ApplicationServices.GetRequiredService<IHostApplicationLifetime>();
            var options = app.ApplicationServices.GetRequiredService<IOptions<ConsulServiceOptions>>().Value;
            //后续可通过依赖注入，将此更改为分布式唯一ID获取方法
            options.ServiceID = Guid.NewGuid().ToString();
            var consulClient = new ConsulClient(configOverride => 
            {
                configOverride.Address = new Uri(options.ConsulAddress);
            });
            var features = app.Properties["server.Features"] as FeatureCollection;
            var address = features.Get<IServerAddressesFeature>().Addresses.First();
            var url = new Uri(address);
            var registerration = new AgentServiceRegistration()
            {
                ID = options.ServiceID,
                Name = options.ServiceName,
                Address = url.Host,
                Port = url.Port,
                Check = new AgentServiceCheck
                {
                    Timeout = TimeSpan.FromSeconds(5),
                    DeregisterCriticalServiceAfter = TimeSpan.FromSeconds(5),
                    HTTP = $"{url.Scheme}://{url.Host}:{url.Port}{options.HealthCheck}",
                    Interval = TimeSpan.FromSeconds(10)
                }
            };
            consulClient.Agent.ServiceRegister(registerration).Wait();
            lifetime.ApplicationStopping.Register(() =>
            {
                consulClient.Agent.ServiceDeregister(options.ServiceID).Wait();
            });
            return app;
        }
    }
}
