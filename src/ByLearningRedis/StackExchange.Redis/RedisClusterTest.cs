using Polly;
using StackExchange.Redis;
using System;
using Xunit;

namespace ByLearningRedis.StackExchange.Redis
{
    public class RedisClusterTest
    {
        private ConnectionMultiplexer connections;
        public RedisClusterTest()
        {
            var policy = Policy.Handle<Exception>().Retry(3, (ex, i) =>
            {
                //记录连接异常
                //设置连接IP及端口
                var info = ex.Message;
            });
            policy.Execute(() =>
            {
                connections = ConnectionMultiplexer.SentinelConnect(new ConfigurationOptions()
                {
                    EndPoints =
                    {
                        { "10.0.3.19", 26379 },
                        { "10.0.3.19", 26380 },
                        { "10.0.3.19", 26381 },
                    },
                });
            });
        }

        [Fact]
        public void SimpleUseTest()
        {
            var server = connections.GetSentinelMasterConnection(new ConfigurationOptions()
            {
                ServiceName = "redis-master",
                //StackExchange.Redis.RedisConnectionException
                Password = "xxxxxxx",
                //StackExchange.Redis.RedisCommandException:“This operation is not available unless admin mode is enabled: ROLE”
                AllowAdmin = true
            });
            var database = server.GetDatabase();
            // hello world   in redis
            var value = database.StringGet("hello");
        }
    }
}
