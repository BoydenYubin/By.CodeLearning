using RabbitMQ.Client;
using Shouldly;
using Xunit;

namespace ByLearningRabbitmq
{
    public class HowToConnectTest
    {
        private ConnectionFactory _factory;
        public HowToConnectTest()
        {
            _factory = BuildConnectionFactory.Create();
        }
        [Fact]
        public void BuildConnectionFactoryTest()
        {
             var connection = _factory.CreateConnection();
            //可创建connection
            connection.ShouldNotBeNull();
            var connection_new = _factory.CreateConnection();
            //两次的连接对象是不同的
            connection.ShouldNotBe(connection_new);
        }
        [Fact]
        public void ExchangeDeclareTest()
        {
            var connection = _factory.CreateConnection();
            var channel = connection.CreateModel();
            /*
             交换器各参数说明：
             exchange:交换器的名称
             type:交换器类型，fanout，direct，topic
             durable：是否持久化
             autoDelete:是否自动删除，自动删除的前提是至少有一个队列或交换
                        器与之绑定，之后所有与这个交换器绑定的队列或交换器
                        都与此解绑，则会自动删除
             arguments:结构化参数
             */
            channel.ExchangeDeclare(exchange: "", type: "direct", durable: false, autoDelete: false, arguments: null);
        }
        [Fact]
        public void QueueDeclareTest()
        {
            var connection = _factory.CreateConnection();
            var channel = connection.CreateModel();
            /*
             各参数说明：
             queue:队列的名称
             durable：是否持久化，排他队列是基于连接可见，连接断开后自动删除
             exclusive:是否排他，当设置队列为排他时，只对首次声明它的连接可见
             autoDelete:是否自动删除
             arguments:结构化参数
             */
            channel.QueueDeclare(queue: "", durable: false, exclusive: true, autoDelete: true, arguments: null);
        }
        [Fact]
        public void QueueBindTest()
        {
            var connection = _factory.CreateConnection();
            var channel = connection.CreateModel();
            channel.QueueDeclare(queue: "", durable: false, exclusive: true, autoDelete: true, arguments: null);
            /*
            各参数说明：
            routingKey:用来绑定队列和交换器的路由键
            arguments:结构化参数
            */
            channel.QueueBind(queue: "", exchange: "", routingKey: "", arguments: null);
        }
        /// <summary>
        /// 交换器可以与交换器绑定
        /// </summary>
        [Fact]
        public void ExhcangeBindTest()
        {
            var connection = _factory.CreateConnection();
            var channel = connection.CreateModel();
            /*
            各参数说明：
            destination:目标交换器
            source：源交换器
            routingKey:用来绑定队列和交换器的路由键
            arguments:结构化参数
            */
            channel.ExchangeBind(destination: "", source: "", routingKey: "", arguments: null);
        }
    }
}
