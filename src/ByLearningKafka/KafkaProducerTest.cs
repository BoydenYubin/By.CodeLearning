using Confluent.Kafka;
using Shouldly;
using System;
using Xunit;

namespace ByLearningKafka
{
    public class KafkaProducerTest
    {
        private ProducerConfig _config;
        public KafkaProducerTest()
        {
            _config = Config.GetProducerConfig();
        }

        [Theory]
        [InlineData(Config.TopicName)]
        public void ProduceMessageTest(string topic)
        {
            using (var producer = new ProducerBuilder<string, string>(_config).Build())
            {
                var result = string.Empty;
                for (int i = 10; i < 20; i++)
                {
                    result = producer.ProduceAsync(topic, new Message<string, string> { Key = $"Key:{i}", Value = $"Value:{i}" })
                        .ContinueWith(task => task.IsFaulted
                            ? $"error producing message: {task.Exception.Message}"
                            : $"produced to: {task.Result.TopicPartitionOffset}").Result;
                }
                result.ShouldContain("produced to:");
                // block until all in-flight produce requests have completed (successfully
                // or otherwise) or 10s has elapsed.
                producer.Flush(TimeSpan.FromSeconds(10));
            }
        }

        [Theory]
        [InlineData(Config.TopicName)]
        public void ProduceMessageWithActionTest(string topic)
        {
            using (var producer = new ProducerBuilder<string, string>(_config).Build())
            {
                for (int i = 121; i < 221; i++)
                {
                    producer.Produce(topic, new Message<string, string> { Key = $"Key:{i}", Value = $"Value:{i}" }, report =>
                    {
                        //Let's check the report
                        //The action is running Asynchronously
                        report.Error.Reason.ToLower().ShouldBe("success");
                        // It will throw the exception
                        // since not method is async, so the report.Key is not match
                        //report.Key.ShouldBe($"Key:{i}");
                    });
                }
                // block until all in-flight produce requests have completed (successfully
                // or otherwise) or 10s has elapsed.
                producer.Flush(TimeSpan.FromSeconds(10));
            }
        }
    }
}
