using RabbitMQ.Client;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Xunit;

namespace ByLearningRabbitmq
{
    public class HowToUseAdvancedTest
    {
        private ConnectionFactory _factory;
        public HowToUseAdvancedTest()
        {
            _factory = BuildConnectionFactory.Create();
        }
        [Fact]
        public void HowToUseMandatoryTest()
        {
            var connection = _factory.CreateConnection();
            var channel = connection.CreateModel();
            channel.BasicReturn += (sender, args) =>
            {
                var message = Encoding.Default.GetString(args.Body.ToArray());
                message.ShouldBe("Been return");
            };
            //mandatory设置为true时
            //当发布的消息无法匹配正确的交换器时，channel会return该消息
            //mandatory设置为false时
            //则直接丢弃消息
            channel.BasicPublish("bylearning.base.test", "direct.to.wrong", mandatory: true, null, Encoding.Default.GetBytes("Been return"));
            Thread.Sleep(2000);
        }
        [Fact]
        public void HowToUseAlternateExchageTest()
        {
            var connection = _factory.CreateConnection();
            var channel = connection.CreateModel();
            var args = new Dictionary<string, object>();
            args.Add("alternate-exchange", "bylearning.adv.ae");
            //定义主交换器
            //定义交换器后，再次修改参数将会报错
            channel.ExchangeDeclare("bylearning.adv.to.ae", "direct", durable: true, autoDelete: true, args);
            //定义备份交换器
            //建议备份交换器需要为fanout类型
            //这是由于消息被重新路由到备份交换器时的路由键和从生产者发出的路由键是一样的。
            channel.ExchangeDeclare("bylearning.adv.ae", "fanout", durable: true, autoDelete: true, null);
            //绑定主交换器与队列
            channel.QueueDeclare("bylearning.adv.to.aeq", true, exclusive: true, autoDelete: false, null);
            channel.QueueBind(queue: "bylearning.adv.to.aeq", exchange: "bylearning.adv.to.ae", "direct.to.ae", null);
            //绑定备份交换器与队列
            channel.QueueDeclare("bylearning.adv.aeq", true, exclusive: true, autoDelete: false, null);
            channel.QueueBind(queue: "bylearning.adv.aeq", exchange: "bylearning.adv.ae", "direct.ae", null);
            //发送正确的消息,10s后过期
            var properties = channel.CreateBasicProperties();
            properties.Expiration = "10000";
            channel.BasicPublish("bylearning.adv.to.ae", "direct.to.ae", mandatory: false, properties, Encoding.Default.GetBytes("Hello World"));
            //发送错误的消息，将被路由至备份路由器
            //当备份路由器和mandatory一起使用时，则mandatory参数无效
            channel.BasicPublish("bylearning.adv.to.ae", "direct.to.ae.wrong", mandatory: false, properties, Encoding.Default.GetBytes("Hello World"));
            Thread.Sleep(20000);
        }
        /// <summary>
        /// 过期队列和过期信息
        /// 需要设置到队列上
        /// </summary>
        [Fact]
        public void HowToUseTTLTest()
        {
            //TTL time to live
            // 1st use directly on the queue
            var args = new Dictionary<string, object>();
            args.Add("x-message-ttl", 6000);
            var connection = _factory.CreateConnection();
            var channel = connection.CreateModel();
            //不设置ttl则代表着永远不过期
            //设置为0则代表着此时如果可以直接将消息投递给消费者，否则直接丢弃           
            channel.QueueDeclare("bylearning.adv.ttl", durable: false, exclusive: true, autoDelete: true, args);
            //若单独给消息设置ttl,则设置CreateBasicProperties
            //发送消息时带此参数
            var properties = channel.CreateBasicProperties();
            properties.Expiration = "6000";
            //过期队列的设置方法
            //队列上没有任何消费者，队列也没有被重新声明
            //过期时间内也没有被使用过Basci.Get命令
            args.Add("x-expires", 18000);
        }
        [Fact]
        public void HowToUseDeadLetterExchage()
        {
            //死信队列
            //消息被拒绝(Rejuce/Nack)
            //消息过期
            //消息达到最大长度
            var args = new Dictionary<string, object>();
            args.Add("x-dead-letter-exchage", "dlx_exchange");
            var connection = _factory.CreateConnection();
            var channel = connection.CreateModel();
            //为队列声明死信队列
            channel.QueueDeclare("bylearning.xld.queue", durable: false, exclusive: true, autoDelete: true, args);
            //同时也可以为这个DLX指定路由键,如果没有特殊指定，则使用原队列的路由键
            args.Add("x-dead-letter-routing-key", "dlx-routing-key");
        }
        /// <summary>
        /// 使用ttl + 死信队列 设置延迟队列；
        /// ttl 以及 死信队列的设置都是针对queue，而不是exchange；
        /// </summary>
        [Fact]
        public void HowToBuildDelayedQueue()
        {
            var connection = _factory.CreateConnection();
            var channel = connection.CreateModel();

            //普通队列交换器，延迟30s
            channel.ExchangeDeclare("bylearning.delay30s.exchange", "direct", durable: false, autoDelete: false, null);
            //延迟队列交换器
            channel.ExchangeDeclare("bylearning.delayed.exchange", "fanout", durable: false, autoDelete: false, null);
            //普通队列，延迟30s
            //设置死信队列
            var args = new Dictionary<string, object>();
            args.Add("x-dead-letter-exchange", "bylearning.delayed.exchange");
            args.Add("x-message-ttl", 10000);
            channel.QueueDeclare("bylearning.delay30s.queue", durable: true, exclusive: false, autoDelete: false, args);
            //延迟队列
            channel.QueueDeclare("bylearning.delayed.queue", durable: true, exclusive: false, autoDelete: false, null);
            //绑定延迟队列及交换器
            channel.QueueBind("bylearning.delay30s.queue", "bylearning.delay30s.exchange", "delay30s", null);
            //绑定消费队列及交换器
            channel.QueueBind("bylearning.delayed.queue", "bylearning.delayed.exchange", "delayed", null);
            channel.BasicPublish("bylearning.delay30s.exchange", "delay30s", mandatory: false, null, Encoding.Default.GetBytes("Hello World"));
            Thread.Sleep(15000);
        }
        [Fact]
        public void HowToUsePriorityQueue()
        {
            var connection = _factory.CreateConnection();
            var channel = connection.CreateModel();
            //声明优先级队列的交换器
            channel.ExchangeDeclare("bylearning.priority.exchage", "direct", durable: true, autoDelete: false, arguments: null);
            //使用参数声明优先级队列
            var args = new Dictionary<string, object>();
            args.Add("x-max-priority", 10);
            channel.QueueDeclare("bylearning.priority.queue", durable: true, exclusive: false, autoDelete: false, args);
            //绑定交换器和队列
            channel.QueueBind("bylearning.priority.queue", "bylearning.priority.exchage", "priority", null);
            //轮询发送
            for (int i = 0; i < 10; i++)
            {
                var properites = channel.CreateBasicProperties();
                properites.Priority = (byte)new Random().Next(0, 9);
                //发错exchange名字会导致连接断开，报错alreadyclosed
                channel.BasicPublish("bylearning.priority.exchage", "priority", properites, Encoding.Default.GetBytes($"Hello World: {i}"));
            }
        }
        /// <summary>
        /// 惰性队列
        /// 解决问题：如果发送端过快或消费端宕机，导致消息大量积压，此时消息还是在内存和磁盘各存储一份，
        /// 在消息大爆发的时候，MQ服务器会撑不住，影响其他队列的消息收发
        /// 解决方式：先存储在磁盘中，占用比较少的内存
        /// https://honeypps.com/mq/rabbitmq-analysis-of-lazy-queue/
        /// </summary>
        [Fact]
        public void HowToUseLazyQueue()
        {
            var connection = _factory.CreateConnection();
            var channel = connection.CreateModel();
            //声明优先级队列的交换器
            channel.ExchangeDeclare("bylearning.lazy.exchage", "direct", durable: true, autoDelete: false, arguments: null);
            //使用参数声明优先级队列
            var args = new Dictionary<string, object>();
            args.Add("x-queue-mod", "lazy");
            channel.QueueDeclare("bylearning.lazy.queue", durable: true, exclusive: false, autoDelete: false, args);
            //绑定交换器和队列
            channel.QueueBind("bylearning.lazy.queue", "bylearning.lazy.exchage", "lazy", null);
            channel.BasicPublish("bylearning.lazy.exchage", "lazy", null, Encoding.Default.GetBytes($"Hello World"));
        }
    }
}