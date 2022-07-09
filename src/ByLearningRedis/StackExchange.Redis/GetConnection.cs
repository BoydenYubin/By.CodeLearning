using Polly;
using StackExchange.Redis;
using System;

namespace ByLearningRedis.StackExchange.Redis
{
    public class GetConnection
    {
        private static ConnectionMultiplexer connections;
        private static string ip = "redis-10216.c278.us-east-1-4.ec2.cloud.redislabs.com";
        private static int port = 10216;
        private static string user = "xxxxxx";
        private static string password = "xxxxxxxxxx";
        public static ConnectionMultiplexer GetConnectionMultiplexer()
        {
            var policy = Policy.Handle<Exception>().Retry(3, (ex, i) =>
            {
                //记录连接异常
                //设置连接IP及端口
                var info = ex.Message;
            });
            policy.Execute(() =>
            {
                connections = ConnectionMultiplexer.Connect(new ConfigurationOptions()
                {
                    EndPoints =
                    {
                        { ip, port }
                    },
                    User = user,
                    Password = password
                });
            });
            return connections;
        }
    }
}
