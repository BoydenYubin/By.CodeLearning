using Shouldly;
using StackExchange.Redis;
using System.Threading;
using Xunit;

namespace ByLearningRedis.StackExchange.Redis
{
    public class SubPubInRedisTest
    {
        private ConnectionMultiplexer connections;
        private IDatabase db;
        public SubPubInRedisTest()
        {
            connections = GetConnection.GetConnectionMultiplexer();
        }
        [Theory]
        [InlineData("sub.test")]
        public void SubscribeTest(string channel)
        {
            var sub = connections.GetSubscriber();
            sub.Subscribe(channel, (channel, value) =>
            {
                channel.IsNullOrEmpty.ShouldBeFalse();
                value.ToString().ShouldBe("test");
            });

            db = connections.GetDatabase();
            db.Publish(channel, "test");
            Thread.Sleep(2000);
        }

        [Theory]
        [InlineData("*.test")]
        public void MultiSubscriberTest(string channel)
        {
            var sub1 = connections.GetSubscriber();
            sub1.Subscribe(channel, (channel, value) =>
            {
                channel.IsNullOrEmpty.ShouldBeFalse();
                value.ToString().ShouldBe("test");
            });

            var sub2 = connections.GetSubscriber();
            sub2.Subscribe(channel, (channel, value) =>
            {
                channel.IsNullOrEmpty.ShouldBeFalse();
                value.ToString().ShouldBe("test");
            });

            db = connections.GetDatabase();
            db.Publish("sub.test", "test");
            Thread.Sleep(5000);
        }
    }
}
