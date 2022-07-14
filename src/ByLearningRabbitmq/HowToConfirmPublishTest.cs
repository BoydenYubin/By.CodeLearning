using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Xunit;

namespace ByLearningRabbitmq
{
    public class HowToConfirmPublishTest
    {
        private ConnectionFactory _factory;
        public HowToConfirmPublishTest()
        {
            _factory = BuildConnectionFactory.Create();
        }

        [Fact]
        public void UseTranstractionTest()
        {
            var connection = _factory.CreateConnection();
            var channel = connection.CreateModel();
            channel.ExchangeDeclare("bylearning.exchange", "direct", true, false);
            channel.QueueDeclare("bylearning.queue", true, false, false);
            channel.QueueBind("bylearning.queue", "bylearning.exchange", "trans.to.queue");

            try
            {
                // transaction
                channel.TxSelect();
                for (int i = 0; i < 10; i++)
                {
                    channel.BasicPublish("bylearning.exchange", "trans.to.queue", body: Encoding.Default.GetBytes($"Hello World{i}"));
                    //if(i == 9)
                    //{
                    //    throw new Exception("purpose");
                    //}
                }
            }
            catch (Exception e)
            {
                //Rollback
                //no message will be sent to rabbitmq
                channel.TxRollback();
            }
        }

        [Fact]
        public void UseConfirmSelectTest()
        {
            var connection = _factory.CreateConnection();
            var channel = connection.CreateModel();
            // it can't be used with TxSelect() at the same time
            channel.ConfirmSelect();
            channel.BasicPublish("bylearning.exchange", "trans.to.queue", body: Encoding.Default.GetBytes($"Hello World!!!"));
            var result = channel.WaitForConfirms();
            Assert.True(result);
        }

        [Fact]
        public void UseAsyncConfirmTest()
        {
            var connection = _factory.CreateConnection();
            var channel = connection.CreateModel();
            SortedSet<ulong> datas = new SortedSet<ulong>();

            channel.BasicAcks += (sender, args) =>
            {
                if (args.Multiple)
                {
                    datas.RemoveWhere(i => i <= args.DeliveryTag);
                }
                else
                {
                    datas.Remove(args.DeliveryTag);
                }
            };
            channel.BasicNacks += (args, multi) =>
            {
                // same logic with the BasicAcks 
                // resent the data logic
            };
            channel.ConfirmSelect();
            for (int i = 101; i < 201; i++)
            {
                ulong seqNo = channel.NextPublishSeqNo;
                var body = Encoding.Default.GetBytes($"Hello World{i}");
                channel.BasicPublish("bylearning.exchange", "trans.to.queue", body: body);
                datas.Add(seqNo);
            }
            Thread.Sleep(1000);
            Assert.True(datas.Count == 0);
        }

        [Fact]
        public void UseBatchConfirmSelectTest()
        {
            var cache = new List<byte[]>();
            var connection = _factory.CreateConnection();
            var channel = connection.CreateModel();
            int batchSize = 10; // size of each batch

            channel.ConfirmSelect();
            for (int i = 1; i < 101; i++)
            {
                var body = Encoding.Default.GetBytes($"Hello World{i}");
                channel.BasicPublish("bylearning.exchange", "trans.to.queue", body: body);
                cache.Add(body);
                // check if fit for the batch size
                if (i % batchSize == 0)
                {
                    var result = channel.WaitForConfirms();
                    if (result)
                    {
                        cache.Clear();  //clear the cache from the cache
                    }
                    else
                    {
                        // resent the data in cache
                    }
                }
            }

        }
    }
}
