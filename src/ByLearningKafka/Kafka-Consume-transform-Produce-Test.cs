using Confluent.Kafka;
using Confluent.Kafka.Admin;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Xunit;

namespace ByLearningKafka
{
    public class Kafka_Consume_transform_Produce_Test
    {
        private ConsumerConfig _cConfig;
        private ProducerConfig _pConfig;
        private const string _destTopic = "des_topic";
        public Kafka_Consume_transform_Produce_Test()
        {
            _cConfig = Config.GetConsumerConfig();
            //enable auto commit == false
            _cConfig.EnableAutoCommit = false;
            _cConfig.ClientId = Thread.CurrentThread.ManagedThreadId + "_consumer";
            _cConfig.GroupId = "ConsumerGroup_Consume_transform_Produce";
            // AutoOffsetReset specifies the action to take when there
            // are no committed offsets for a partition, or an error
            // occurs retrieving offsets. If there are committed offsets,
            // it has no effect.
            _cConfig.AutoOffsetReset = AutoOffsetReset.Earliest;
            // Offsets are committed using the producer as part of the
            // transaction - not the consumer. When using transactions,
            // you must turn off auto commit on the consumer, which is
            // enabled by default!
            _cConfig.EnableAutoCommit = false;
            // Enable incremental rebalancing by using the CooperativeSticky
            // assignor (avoid stop-the-world rebalances).
            _cConfig.PartitionAssignmentStrategy = PartitionAssignmentStrategy.CooperativeSticky;
            _pConfig = Config.GetProducerConfig();
            _pConfig.ClientId = Thread.CurrentThread.ManagedThreadId + "_producer";
            // The TransactionalId identifies this instance of the map words processor.
            // If you start another instance with the same transactional id, the existing
            // instance will be fenced.
            _pConfig.TransactionalId = "TransactionalIdPrefix_MapWords" + "-" + Thread.CurrentThread.ManagedThreadId;
        }

        [Theory]
        [InlineData(_destTopic)]
        //run this one time
        public void ProduceDestinationTopicTest(string desTopic)
        {
            var adminConfig = Config.GetAdminClientConfig();
            using (var admin = new AdminClientBuilder(adminConfig).Build())
            {
                var createResult = admin.CreateTopicsAsync(new List<TopicSpecification>()
                {
                    new TopicSpecification()
                    {
                        Name = desTopic,
                        NumPartitions = 3,
                        ReplicationFactor = 2
                    }
                }).ContinueWith(task => !task.IsFaulted ? true : false);
                createResult.Result.ShouldBe(true);
            }
        }

        [Theory]
        [InlineData(Config.TopicName, _destTopic)]
        public void ProduceAndConsumeSthTest(string srcTopic, string desTopic)
        {
            var adminConfig = Config.GetAdminClientConfig();
            using (var admin = new AdminClientBuilder(adminConfig).Build())
            {
                //Consume some and transform to another topic
                using (var producer = new ProducerBuilder<string, string>(_pConfig).Build())
                using (var consumer = new ConsumerBuilder<string, string>(_cConfig).Build())
                {
                    //消费者主动消费源topic数据，然后下发到目标topic
                    consumer.Subscribe(srcTopic);
                    producer.InitTransactions(TimeSpan.FromSeconds(30));
                    producer.BeginTransaction();
                    try
                    {
                        int i = 0;
                        while (i < 20)
                        {
                            var result = consumer.Consume(TimeSpan.FromSeconds(10));
                            
                            try
                            {
                                producer.Produce(desTopic, result.Message);
                                i++;
                            }
                            catch (KafkaException e)
                            {
                                if (e.Error.Code == ErrorCode.Local_QueueFull)
                                {
                                    Thread.Sleep(TimeSpan.FromMilliseconds(1000));
                                    continue;
                                }
                                throw;
                            }
                        }
                        //producer将生产者和消费者的数据同时标记为事务状态，用以提交，否则消费也失败
                        producer.SendOffsetsToTransaction(consumer.Assignment.Select(a => new TopicPartitionOffset(a, consumer.Position(a))),
                        consumer.ConsumerGroupMetadata,
                        TimeSpan.FromSeconds(30));
                        // 如果未提交事务之前出现exception，则消费和生产相关的位移均被abort
                        //throw new Exception("test");
                        producer.CommitTransaction();
                        var positionAfter = consumer.Position(new TopicPartition(srcTopic, new Partition(0)));
                    }
                    catch (Exception e)
                    {
                        // Attempt to abort the transaction (but ignore any errors) as a measure
                        // against stalling consumption of Topic_Words.
                        producer.AbortTransaction();
                        consumer.Close();
                    }
                }
            }

        }
    }
}
