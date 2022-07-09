using Confluent.Kafka;

namespace ByLearningKafka
{
    public static class Config
    {
        // Use Confluent Cloud to Create Kafka cluster to use the BootstrapServers
        // https://confluent.cloud
        // relpace the string to use your confluent
        public static string BootstrapServers = "xxxx";
        public static string SaslUsername = "xxxx";
        public static string SaslPassword = "xxxx";

        public const string TopicName = "bykafka_first_test_topic";

        private static ClientConfig GetClinetConfig()
        {
            return new ClientConfig
            {
                BootstrapServers = BootstrapServers,
                SaslUsername = SaslUsername,
                SaslPassword = SaslPassword,
                SaslMechanism = SaslMechanism.Plain,
                SecurityProtocol = SecurityProtocol.SaslSsl
            };
        }

        public static AdminClientConfig GetAdminClientConfig()
        {
            return new AdminClientConfig(GetClinetConfig());
        }

        public static ProducerConfig GetProducerConfig()
        {
            return new ProducerConfig(GetClinetConfig());
        }
        
        public static ConsumerConfig GetConsumerConfig()
        {
            string id = "32647c95-992c-451e-ba60-2eb77c32c149";
            return new ConsumerConfig(GetClinetConfig())
            {
                GroupId = id,
                AutoOffsetReset = AutoOffsetReset.Earliest
            };
        }
    }
}
