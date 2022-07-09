using Shouldly;
using StackExchange.Redis;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace ByLearningRedis
{
    public class SimpleUseTest
    {
        private ConnectionMultiplexer connections;
        private IDatabase db;
        public SimpleUseTest()
        {
            connections = StackExchange.Redis.GetConnection.GetConnectionMultiplexer();
        }
        [Fact]
        public void ConnectAndStringSetTest()
        {
            db = connections.GetDatabase();
            db.StringSet("ping", "pong");
            var result = db.StringGet("ping");
            result.ToString().ShouldBe("pong");
            db.KeyDelete("ping");
            db.StringGet("ping").HasValue.ShouldBeFalse();
        }
        [Fact]
        public void IncrTest()
        {
            db = connections.GetDatabase();
            db.StringSet("order", 1);
            Parallel.For(0, 5, i =>
            {
                db.StringIncrement("order", 1);
            });
            var result = (int)db.StringGet("order");
            result.ShouldBe(6);
            db.KeyDelete("order").ShouldBe(true);
            db.StringGet("order").HasValue.ShouldBe(false);
        }
        [Fact]
        public void HowToUseTransactionTest()
        {
            db = connections.GetDatabase();
            var testKey = "UniqueID";
            var id = Guid.NewGuid().ToString();
            var trans = db.CreateTransaction();
            trans.AddCondition(Condition.HashNotExists("test", testKey));
            trans.HashSetAsync("test", testKey, id);
            var committed = trans.Execute();
            committed.ShouldBe(true);
            trans = db.CreateTransaction();
            trans.AddCondition(Condition.HashNotExists("test", testKey));
            trans.HashSetAsync("test", testKey, id);
            committed = trans.Execute();
            committed.ShouldBe(false);
            db.KeyDelete("test").ShouldBe(true);
        }
        /// <summary>
        /// 如何获取单库所有的键值
        /// </summary>
        [Fact]
        public void HowToGetKeysTest()
        {
            var db = connections.GetDatabase();
            var trans = db.CreateTransaction();
            trans.SetAddAsync("A", "A");
            trans.SetAddAsync("B", "B");
            var server = connections.GetServer(connections.GetEndPoints().FirstOrDefault());
            server.Keys().Count().ShouldBe(0); //未提交时数据为0
            var committed = trans.Execute();
            server.Keys().Count().ShouldBeGreaterThan(0);
            committed.ShouldBeTrue();
            db.KeyDelete("A");
            db.KeyDelete("B");
            server.Keys().Count().ShouldBe(0);
        }
        /// <summary>
        /// 使用管道测试
        /// </summary>
        [Fact]
        public void HowToUsePipeLineTest()
        {
            var db = connections.GetDatabase();
            var pipeline = db.CreateBatch();
            pipeline.StringSetAsync("A", "A");
            pipeline.HashSetAsync("B", "Name", "Name");
            pipeline.HashSetAsync("B", "Age", 12);
            pipeline.Execute();
            Assert.Equal("Name", db.HashGet("B", "Name"));
            Assert.Equal("A", db.StringGet("A"));
            db.KeyDelete("A").ShouldBeTrue();
            db.KeyDelete("B").ShouldBeTrue();
        }
    }
}
