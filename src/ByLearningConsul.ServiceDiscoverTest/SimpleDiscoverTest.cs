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

        [Fact]
        public async void ConsulKVTest()
        {
            var address = DemoConsulConfigSettings.ConsulAddress;
            var provicer = new DefaultConsulServiceDiscover(new Uri(address));
            var result = await provicer.PutKVpairs("hello", "world");
            result.ShouldBe(true);
            var response = await provicer.GetKVvalues("hello");
            response.ShouldBe("world");
        }
    }
}
