using ByLearningORM.Util;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ByLearningEFCore.StrategyClass
{
    public class LoggerClass
    {
        public int ID { get; set; }
        public string Name { get; set; }
    }
    public class LoggerDbContext : DbContext
    {
        public DbSet<LoggerClass> Loggers { get; set; }
        public LoggerDbContext()
        {
            Database.Migrate();
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //使用mysql作为测试数据库
            optionsBuilder.UseMySql(GetConfig.GetConnectionString());
            var looger = LoggerFactory.Create(config =>
            {
                config.AddDebug();
            });
            optionsBuilder.UseLoggerFactory(looger);
            //以下语句将会记录参数值
            optionsBuilder.EnableSensitiveDataLogging();
        }
    }
}
