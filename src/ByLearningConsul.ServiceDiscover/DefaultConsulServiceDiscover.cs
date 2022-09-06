using Consul;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ByLearningConsul.ServiceDiscover
{
    public class DefaultConsulServiceDiscover : IConsulServiceProvider
    {
        private readonly ConsulClient _consulClient;
        public DefaultConsulServiceDiscover(Uri url)
        {
            _consulClient = new ConsulClient(config => 
            {
                config.Address = url;
            });
        }
        public async Task<IList<string>> GetServices(string serviceName)
        {
            var queryResult = await _consulClient.Health.Service(service: serviceName, tag: "", passingOnly: true);
            var result = new List<string>();
            foreach (var service in queryResult.Response)
            {
                result.Add(service.Service.Address + ":" + service.Service.Port);
            }
            return result;
        }
    }
}
