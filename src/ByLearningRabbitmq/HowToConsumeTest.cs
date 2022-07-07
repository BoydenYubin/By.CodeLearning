using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Shouldly;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace ByLearningRabbitmq
{
    public class HowToConsumeTest
    {
        private ConnectionFactory _factory;
        public HowToConsumeTest()
        {
            _factory = BuildConnectionFactory.Create();
        }
        /// <summary>
        /// 推模式消费消息
        /// 由服务端处理
        /// </summary>
        [Fact]
        public void HowToConsumeMessageTest()
        {
            var connection = _factory.CreateConnection();
            var channel = connection.CreateModel();
            var asyncConsumer = new AsyncEventingBasicConsumer(channel);
            asyncConsumer.Received += (sender, basicDe) =>
            {
                var body = basicDe.Body.ToArray();
                var message = Encoding.Default.GetString(body);
                message.ShouldBe("Hello World");
                if (message == "Hello World")
                {
                    var channel = ((AsyncEventingBasicConsumer)sender).Model;
                    channel.BasicAck(basicDe.DeliveryTag, false);
                }
                //if (message == "Hello World+")
                //{
                //    var channel = ((AsyncEventingBasicConsumer)sender).Model;
                    // multiple 为true时，代表将此队列之前所有已经消费的消息给应答了
                    // M5  M4  M3  M2  M1
                    // 当M1 - M4 消费后未应答
                    // 在M5时应答，并设置multiple为true，则会将M1-M4全部应答
                //    channel.BasicAck(basicDe.DeliveryTag, multiple: false);
                //}
                return Task.CompletedTask;
            };
            //多线程模式下，要使用异步消费者
            //var consumer = new EventingBasicConsumer(channel);
            //System.InvalidOperationException:“In the async mode you have to use an async consumer”
            //channel.BasicConsume("bylearning.base.queue", false, consumer);
            channel.BasicConsume("bylearning.base.queue", false, asyncConsumer);
            Thread.Sleep(5000);
        }
        /// <summary>
        /// 拉模式消费消息
        /// 由客户端管理
        /// </summary>
        [Fact]
        public void HowToConsumeMessageByClientTest()
        {
            var connection = _factory.CreateConnection();
            var channel = connection.CreateModel();
            var getResult = channel.BasicGet("bylearning.base.queue", true);
            var message = Encoding.Default.GetString(getResult.Body.ToArray());
            message.ShouldBe("Hello World");
        }
    }
}
