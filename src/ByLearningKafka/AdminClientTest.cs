using Confluent.Kafka;
using System;
using Xunit;
using Shouldly;
using System.Collections.Generic;
using Confluent.Kafka.Admin;
using System.Threading.Tasks;
using System.Threading;

namespace ByLearningKafka
{
    public class AdminClientTest
    {
        private AdminClientConfig _config;
        public AdminClientTest()
        {
            _config = Config.GetAdminClientConfig();
        }
        [Fact]
        public void BuildAdminClientTest()
        {
            using (var adminClient = new AdminClientBuilder(_config).Build())
            {
                adminClient.ShouldNotBeNull();
            }
        }
        [Theory]
        [InlineData(Config.TopicName)]
        public void GreateTopicTest(string topic)
        {
            using (var adminClient = new AdminClientBuilder(_config).Build())
            {
                var topics = new List<TopicSpecification>()
                {
                    new TopicSpecification()
                    {
                        Name = topic,
                        NumPartitions = 3,
                        //this must set to be 3, or it will throw the excpetion
                        ReplicationFactor = 3
                    }
                };
                try
                {
                    adminClient.CreateTopicsAsync(topics)
                               .ContinueWith(task =>
                               {
                                   task.IsFaulted.ShouldBeFalse();
                               });
                }
                catch (CreateTopicsException e)
                {
                    e.Error.ShouldNotBeNull();
                }
            }
        }

        [Theory]
        [InlineData(Config.TopicName)]
        public void GetMetaDataTest(string topic)
        {
            using (var adminClient = new AdminClientBuilder(_config).Build())
            {
                // Warning: The API for this functionality is subject to change.
                var meta = adminClient.GetMetadata(TimeSpan.FromSeconds(20));
                // Get the OriginatingBrokerId and OriginatingBrokerName
                var OriginatingBrokerId = meta.OriginatingBrokerId;
                var OriginatingBrokerName = meta.OriginatingBrokerName;
                List<Tuple<int, string, int>> brokes = new List<Tuple<int, string, int>>();
                meta.Brokers.ForEach(broker =>
                {
                    brokes.Add(new Tuple<int, string, int>(broker.BrokerId, broker.Host, broker.Port));
                });
                var topics = new Dictionary<string, List<PartitionMetadata>>();
                meta.Topics.ForEach(topic =>
                {
                    topics.Add(topic.Topic, topic.Partitions);
                });
                topics.ShouldContainKey(topic);
            }
        }

        [Fact]
        public void GetGroupInfoTest()
        {
            //list all the blocked group info in the cluster
            using (var adminClient = new AdminClientBuilder(_config).Build())
            {
                try
                {
                    var topics = new List<TopicSpecification>()
                    {
                         new TopicSpecification()
                         {
                             Name = "tmp_kafka_topic",
                             NumPartitions = 1,
                             //this must set to be 3, or it will throw the excpetion
                             ReplicationFactor = 3
                         }
                     };
                    adminClient.CreateTopicsAsync(new List<TopicSpecification>(topics))
                        .ContinueWith(task => task.IsFaulted.ShouldBeFalse());
                }
                catch(CreateTopicsException e)
                {
                    // this should not be here
                    e.Error.ShouldNotBeNull();
                }
                Thread.Sleep(1000);
                Task.Run(() =>
                {
                    //make a block client
                    var pConfig = Config.GetProducerConfig();
                    var producer = new ProducerBuilder<int, string>(pConfig).Build();
                    producer.Produce("tmp_kafka_topic",
                        new Message<int, string> { Key = 1, Value = "Value:1" });

                    var cConfig = Config.GetConsumerConfig();
                    var consume = new ConsumerBuilder<int, string>(cConfig).Build();
                    consume.Subscribe("tmp_kafka_topic");
                    for (int i = 0; i < 2; i++)
                    {
                        var result = consume.Consume();
                    }
                    consume.Close();
                });
                //wait for the client to block
                Thread.Sleep(3000);
                var groups = adminClient.ListGroups(TimeSpan.FromMinutes(1));
                groups.Count.ShouldBe(1);
                adminClient.DeleteTopicsAsync(new List<string> { "tmp_kafka_topic" })
                    .ContinueWith(task => task.IsFaulted.ShouldBe(false));
            }
        }

    }
}
