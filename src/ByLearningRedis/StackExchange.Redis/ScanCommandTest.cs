using Shouldly;
using StackExchange.Redis;
using System.Diagnostics;
using System.Linq;
using Xunit;

namespace ByLearningRedis.StackExchange.Redis
{
    public class ScanCommandTest
    {
        private ConnectionMultiplexer connections;
        private IDatabase db;
        public ScanCommandTest()
        {
            connections = GetConnection.GetConnectionMultiplexer();
        }
        [Fact]
        public void HScanTest()
        {
            //Seed Data
            db = connections.GetDatabase();

            var batch = db.CreateBatch();
            for (int i = 0; i < 1300; i++)
            {
                batch.HashSetAsync("hsan:test", $"field:{i}", i);
            }
            batch.Execute();
            db.HashGet("hsan:test", "field:12").HasValue.ShouldBeTrue();
            Stopwatch sw = new Stopwatch();
            sw.Start();
            var result = db.HashScan("hsan:test", "field*", cursor: 0, pageSize: 201).Take(400).ToList();
            sw.Stop();
            
            db.KeyDelete("hsan:test").ShouldBeTrue();
        }
        [Fact]
        public void ScanTest()
        {
            var str_key = "Scan:Str";
            var hash_key = "Scan:Hash";
            //Seed Data
            db = connections.GetDatabase();
            var batch = db.CreateBatch();
            for (int i = 0; i < 1000; i++)
            {
                batch.StringSetAsync($"{str_key}:{i}", i);
            }
            for (int i = 0; i < 1000; i++)
            {
                batch.HashSetAsync(hash_key, $"{hash_key}:{i}", i);
            }
            batch.Execute();
            //扫描所有的Key需要从server上获取
            var server = connections.GetServer(connections.GetEndPoints().FirstOrDefault());
            var keys = server.Keys(pattern: "Scan*", pageSize: 100, cursor: 0);
            keys.Take(40).Count().ShouldBe(40);
            //删除所有键值
            for (int i = 0; i < 1000; i++)
            {
                batch.KeyDeleteAsync($"{str_key}:{i}");
            }
            batch.KeyDeleteAsync(hash_key);
            batch.Execute();
            server.Keys(pattern: "Scan*", pageSize: 100, cursor: 0).Count().ShouldBe(0);
        }
    }
}
