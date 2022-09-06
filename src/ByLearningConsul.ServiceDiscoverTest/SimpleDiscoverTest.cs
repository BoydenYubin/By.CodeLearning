using ByLearningConsul.ServiceDiscover;
using DemoConsulConfig;
using Shouldly;
using System;
using Xunit;

namespace ByLearningConsul.ServiceDiscoverTest
{
    public class SimpleDiscoverTest
    {
        [Fact]
        public async void ServiceDiscoverTest()
        {
            //should initial two or more ByLearningConsul web service
            var address = DemoConsulConfigSettings.ConsulAddress;
            var provicer = new DefaultConsulServiceDiscover(new Uri(address));
            var services = await provicer.GetServices("ByLearningMicroService");
            services.Count.ShouldBeGreaterThan(0);
        }
    }
}
