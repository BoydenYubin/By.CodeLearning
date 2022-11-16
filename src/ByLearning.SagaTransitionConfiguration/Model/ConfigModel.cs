namespace ByLearning.SagaTransitionConfiguration.Model
{
    public class GlobalSettings
    {
        public RabbitMqConfiguration RabbitMqConfiguration { get; set; }
        public RedisServerConfiguration RedisServerConfiguration { get; set; }
        public int PrefetchCount { get; set; }
        public int ConcurrentMessageLimit { get; set; }
        public int StockNumber { get; set; }
    }

    public class RabbitMqConfiguration
    {
        public string Broker_Address { get; set; }
        public ushort Broker_Port { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string VirtualHost { get; set; }
    }

    public class RedisServerConfiguration
    {
        public string Server_Address { get; set; }
        public ushort Server_Port { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
