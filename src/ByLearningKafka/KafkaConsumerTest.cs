using Confluent.Kafka;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace ByLearningKafka
{
    public class KafkaConsumerTest
    {
        private ConsumerConfig _config;
        public KafkaConsumerTest()
        {
            _config = Config.GetConsumerConfig();
            //enable auto commit == false
            _config.EnableAutoCommit = false;
        }

        [Theory]
        [InlineData(Config.TopicName)]
        public void CreateConsumerTest(string topic)
        {
            using (var consumer = new ConsumerBuilder<string, string>(_config).Build())
            {
                consumer.Subscribe(topic);
                try
                {
                    for (int i = 0; i < 15; i++)
                    {
                        //the clinet will be blocked by the method
                        //we should consume one by one
                        var consumerResult = consumer.Consume();
                    }
                    //the result will display all the topicpartition info 
                    var result = consumer.Commit();
                }
                catch (ConsumeException)
                {
                    //if catch consume excpetion
                }
            }
        }
        [Theory]
        [InlineData(Config.TopicName)]
        public void ConsumerGroupTest(string topic)
        {
            _config.GroupId = "consumer_group";
            //each consumer will consume one parittion with special cousumer_id
            Parallel.For(0, 5, i =>
            {
                using (var consumer = new ConsumerBuilder<string, string>(_config).Build())
                {
                    consumer.Subscribe(topic);
                    Thread.Sleep(1000);
                    for (int j = 0; j < 5; j++)
                    {
                        var consumerResult = consumer.Consume();
                    }
                    var result = consumer.Commit();
                }
            });
            Thread.Sleep(2000);
        }
    }
}
