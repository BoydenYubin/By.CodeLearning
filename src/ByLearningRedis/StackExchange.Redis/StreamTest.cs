using ByLearningRedis.StackExchange.Redis;
using Shouldly;
using StackExchange.Redis;
using System.Threading;
using Xunit;

namespace ByLearningRedis
{
    public class StreamTest
    {
        private ConnectionMultiplexer connections;
        private IDatabase db;
        public StreamTest()
        {
            connections = GetConnection.GetConnectionMultiplexer();
        }

        [Theory]
        [InlineData("stream.queue")]
        public void StreamaConsumeTest(string key)
        {
            db = connections.GetDatabase();

            for (int i = 0; i < 5; i++)
            {
                db.StreamAdd(key, $"newField{i}", $"newValue{i}");
            }

            db.StreamLength(key).ShouldBe(5L);
            //The maximum number of messages to return. ----2
            var readResult = db.StreamRead(key, 0, count: 2);
            readResult.Length.ShouldBe(2);
            //delete the stream key
            db.KeyDelete(key).ShouldBeTrue();
        }

        [Theory]
        [InlineData("stream.queue")]
        public void ConsumeGroupTest(string key)
        {
            db = connections.GetDatabase();
            #region Add stream info
            for (int i = 0; i < 10; i++)
            {
                db.StreamAdd(key, $"newField{i}", $"newValue{i}");
            }
            db.StreamLength(key).ShouldBe(10L);
            #endregion
            #region StreamPosition Flag
            //create the consume group
            ///<seealso cref="StreamPosition.NewMessages"/>  "$"
            //The "$" value used in the XGROUP command.Indicates reading only new messages
            //from the stream.
            ///<seealso cref="StreamPosition.Beginning"/>  "-"
            //The "-" value used in the XRANGE, XREAD, and XREADGROUP commands. Indicates the
            //minimum message ID from the stream.
            #endregion
            db.StreamCreateConsumerGroup(key, "group1", position: StreamPosition.Beginning).ShouldBeTrue();
            //group consume the message
            ///<seealso cref="StreamConstants.UndeliveredMessages"/>  ">"
            // The ">" value used in the XREADGROUP command. Use this to read messages that
            // have not been delivered to a consumer group.
            var list = new System.Collections.Concurrent.ConcurrentQueue<StreamEntry>();
            System.Threading.Tasks.Parallel.For(0, 2, i =>
            {
                var result = db.StreamReadGroup(key: key, groupName: "group1", consumerName: $"cousumer{i}", count: 5, position: ">", noAck: true);
                result.Length.ShouldBe(5);
                foreach (var entry in result)
                {
                    list.Enqueue(entry);
                }
            });
            list.Count.ShouldBe(10);
            Thread.Sleep(3000);
            db.StreamReadGroup(key: key, groupName: "group1", consumerName: "cousumerA", count: 5, position: ">", noAck: true).Length.ShouldBe(0);
            db.KeyDelete(key).ShouldBeTrue();
        }

        [Theory]
        [InlineData("stream.queue")]
        public void ConsumeNewMeaageTest(string key)
        {
            db = connections.GetDatabase();
            #region Add stream info
            for (int i = 0; i < 10; i++)
            {
                db.StreamAdd(key, $"newField{i}", $"newValue{i}");
            }
            db.StreamLength(key).ShouldBe(10L);
            #endregion
            db.StreamCreateConsumerGroup(key, "group1", position: StreamPosition.NewMessages).ShouldBeTrue();
            // all the message from the beginging cann't be 
            // "-" means the min position of the group
            // db.StreamReadGroup(key: key, groupName: "group1", consumerName: "cousumerA", count: 5, position: "-", noAck: true).Length.ShouldBe(0);
            db.StreamReadGroup(key: key, groupName: "group1", consumerName: "cousumerA", count: 5, position: ">", noAck: true).Length.ShouldBe(0);
            for (int i = 10; i < 13; i++)
            {
                db.StreamAdd(key, $"newField{i}", $"newValue{i}");
            }
            db.StreamReadGroup(key: key, groupName: "group1", consumerName: "cousumerA", count: 3, position: ">", noAck: true).Length.ShouldBe(3);
            db.KeyDelete(key).ShouldBeTrue();
        }

        #region Fig to show the pending
        /// -----------------------------------------------------------------
        ///  |      I  H  G  F  E    |    Stream  
        ///  ----------------------------------------------------------------
        ///  |           B  A        |    Group-Consumer1    Read from stream
        /// -----------------------------------------------------------------
        ///  |           D  C        |    Group-Consumer2    Read from stream  
        /// -----------------------------------------------------------------
        ///  |           B  A        |    Group-Consumer1    Pending status
        ///  |           D  C        |    Group-Consumer2    Pending status 
        /// -----------------------------------------------------------------
        /// -----------------------------------------------------------------
        ///  |           I           |    Stream  
        ///  ----------------------------------------------------------------
        ///       read        ack 
        ///  |    F  E   |   B  A    |  Group-Consumer1    Read and ack
        /// -----------------------------------------------------------------
        ///       read        ack 
        ///  |  H  G  D  |      C    |  Group-Consumer2    Read and ack 
        /// -----------------------------------------------------------------
        ///  |           F  E        |    Group-Consumer1    Pending status
        ///  |        H  G  D        |    Group-Consumer2    Pending status 
        /// -----------------------------------------------------------------
        #endregion
        [Theory]
        [InlineData("stream.queue")]
        public void StreamPendingTest(string key)
        {
            db = connections.GetDatabase();
            #region Add stream info
            for (int i = 0; i < 10; i++)
            {
                db.StreamAdd(key, $"newField{i}", $"newValue{i}");
            }
            db.StreamLength(key).ShouldBe(10L);
            #endregion
            db.StreamCreateConsumerGroup(key, "group1", position: StreamPosition.Beginning).ShouldBeTrue();
            //Once a message has been read by a consumer its state becomes “pending” for the consumer, no other consumer can read that message
            //noAck: When true, the message will not be added to the pending message list.
            //db.StreamReadGroup(key: key, groupName: "group1", consumerName: "cousumerA", count: 5, position: ">", noAck: true)
            db.StreamReadGroup(key: key, groupName: "group1", consumerName: "cousumerA", count: 5, position: ">").Length.ShouldBe(5);
            // for same consumer the message has been read can't read again, ">" will not work for same consumer
            // The "0-0" value used in the XREADGROUP command. Use this to read messages that
            // have been delivered to a consumer group. so that can get the pending messages
            var consumePending = db.StreamReadGroup(key: key, groupName: "group1", consumerName: "cousumerA", count: 3, position: "0-0");
            //ackknowledage
            foreach (var message in consumePending)
            {
                db.StreamAcknowledge(key, "group1", message.Id);
            }
            // also can get the pending message by below method
            var pendingMessages = db.StreamPendingMessages(key, "group1", 5, "cousumerA");
            //ackknowledage
            foreach (var message in pendingMessages)
            {
                db.StreamAcknowledge(key, "group1", message.MessageId);
            }
            db.KeyDelete(key).ShouldBeTrue();
        }
    }
}
