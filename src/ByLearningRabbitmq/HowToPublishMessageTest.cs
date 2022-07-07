using RabbitMQ.Client;
using System.Text;
using Xunit;

namespace ByLearningRabbitmq
{
    public class HowToPublishMessageTest
    {
        private ConnectionFactory _factory;
        public HowToPublishMessageTest()
        {
            _factory = BuildConnectionFactory.Create();
        }
        [Fact]
        public void PublishMessageTest()
        {
            var connection = _factory.CreateConnection();
            var channel = connection.CreateModel();
            channel.QueueDeclare("bylearning.base.queue", true, false, false);
            channel.QueueBind("bylearning.base.queue", "bylearning.base.test", "direct.to");
            channel.BasicPublish(exchange: "bylearning.base.test", routingKey: "direct.to", mandatory: false, basicProperties: null, body: Encoding.Default.GetBytes("Hello World"));
            //channel.BasicPublish(addr: null, basicProperties: null, body: null);
            var properties = channel.CreateBasicProperties();
            properties.DeliveryMode = 1;  //持久化为1，非持久化为2
            properties.Priority = 0; //优先级
            //properties.Timestamp   //消息时间戳
            //properties.Expiration   //过期时间
            //properties.Headers
        }
    }
}
