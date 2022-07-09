using Shouldly;
using StackExchange.Redis;
using System.Diagnostics;
using Xunit;

namespace ByLearningRedis.StackExchange.Redis
{
    public class BitOperationTest
    {
        private ConnectionMultiplexer connections;
        public BitOperationTest()
        {
            connections = GetConnection.GetConnectionMultiplexer();
        }

        [Theory]
        [InlineData("bit_operation")]
        public void SetAndGetBitTest(string key)
        {
            var db = connections.GetDatabase();
            for (int i = 0; i < 120; i++)
            {
                if(i % 3 == 0)
                {
                    //this is slow, don't set too much number
                    db.StringSetBit(key, i, true);
                }
            }
            db.StringGetBit(key, 33).ShouldBe(true);
            db.StringGetBit(key, 66).ShouldBe(true);
            db.StringGetBit(key, 99).ShouldBe(true);
            //get the bit count
            db.StringBitCount(key, 0, -1).ShouldBe(40);
            db.KeyDelete(key).ShouldBe(true);
        }

        [Theory]
        [InlineData(20)]
        public void BachSetBitTest(int count)
        {
            var db = connections.GetDatabase();
            string key = "bach.setbit";
            var sw = new Stopwatch();
            //just use the for to setbit
            sw.Start();
            for (int i = 0; i < count; i++)
            {
                db.StringSetBit(key, i, true);
            }
            sw.Stop();
            var time_for = sw.ElapsedMilliseconds;
            sw.Reset();
            //use pipeline to setbit
            var pipline = db.CreateBatch();
            sw.Start();
            for (int i = 0; i < count; i++)
            {
                pipline.StringSetBitAsync(key, i, true);
            }
            pipline.Execute();
            sw.Stop();
            var time_bach = sw.ElapsedMilliseconds;
            time_bach.ShouldBeLessThan(time_for);
            db.KeyDelete(key);
        }

        [Fact]
        public void StringBitOperationTest()
        {
            string key1 = "string.bitoperation1";
            string key2 = "string.bitoperation2";
            // data
            var db = connections.GetDatabase();
            for (int i = 0; i < 30; i++)
            {
                if(i % 2 == 0)
                {
                    db.StringSetBit(key1, i, true);
                }
                else
                {
                    db.StringSetBit(key2, i, true);
                }
            }

            db.StringBitOperation(Bitwise.And, "dst", key1, key2);
            db.StringGetBit("dst", new System.Random().Next(0, 29)).ShouldBeFalse();

            db.StringBitOperation(Bitwise.Or, "dst", key1, key2);
            db.StringGetBit("dst", new System.Random().Next(0, 29)).ShouldBeTrue();

            db.StringBitOperation(Bitwise.Xor, "dst", key1, key2);
            db.StringGetBit("dst", new System.Random().Next(0, 29)).ShouldBeTrue();

            db.StringBitOperation(Bitwise.Not, "dst", key1, key2);
            db.KeyDelete(new RedisKey[] { key1, key2, "dst" }).ShouldBe(3);
        }
    }
}
