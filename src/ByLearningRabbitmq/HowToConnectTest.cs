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
            //�ɴ���connection
            connection.ShouldNotBeNull();
            var connection_new = _factory.CreateConnection();
            //���ε����Ӷ����ǲ�ͬ��
            connection.ShouldNotBe(connection_new);
        }
        [Fact]
        public void ExchangeDeclareTest()
        {
            var connection = _factory.CreateConnection();
            var channel = connection.CreateModel();
            /*
             ������������˵����
             exchange:������������
             type:���������ͣ�fanout��direct��topic
             durable���Ƿ�־û�
             autoDelete:�Ƿ��Զ�ɾ�����Զ�ɾ����ǰ����������һ�����л򽻻�
                        ����֮�󶨣�֮������������������󶨵Ķ��л򽻻���
                        ����˽������Զ�ɾ��
             arguments:�ṹ������
             */
            channel.ExchangeDeclare(exchange: "", type: "direct", durable: false, autoDelete: false, arguments: null);
        }
        [Fact]
        public void QueueDeclareTest()
        {
            var connection = _factory.CreateConnection();
            var channel = connection.CreateModel();
            /*
             ������˵����
             queue:���е�����
             durable���Ƿ�־û������������ǻ������ӿɼ������ӶϿ����Զ�ɾ��
             exclusive:�Ƿ������������ö���Ϊ����ʱ��ֻ���״������������ӿɼ�
             autoDelete:�Ƿ��Զ�ɾ��
             arguments:�ṹ������
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
            ������˵����
            routingKey:�����󶨶��кͽ�������·�ɼ�
            arguments:�ṹ������
            */
            channel.QueueBind(queue: "", exchange: "", routingKey: "", arguments: null);
        }
        /// <summary>
        /// �����������뽻������
        /// </summary>
        [Fact]
        public void ExhcangeBindTest()
        {
            var connection = _factory.CreateConnection();
            var channel = connection.CreateModel();
            /*
            ������˵����
            destination:Ŀ�꽻����
            source��Դ������
            routingKey:�����󶨶��кͽ�������·�ɼ�
            arguments:�ṹ������
            */
            channel.ExchangeBind(destination: "", source: "", routingKey: "", arguments: null);
        }
    }
}
