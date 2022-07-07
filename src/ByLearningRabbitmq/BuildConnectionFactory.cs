using RabbitMQ.Client;

namespace ByLearningRabbitmq
{
    public class BuildConnectionFactory
    {
        public static ConnectionFactory Create()
        {
            //rabbitmq有两个端口
            //15671 15672用来监听用户管理界面
            //5671 5672用来建立消息连接
            //因此docker 运行需要映射多个端口
            return new ConnectionFactory()
            {
                UserName = "xxxx",
                Password = "xxxxxxx",
                HostName = "xxxxxxxxxxxx",
                VirtualHost = "/",
                Port = 5762,
                DispatchConsumersAsync = true
            };
        }
    }
}
