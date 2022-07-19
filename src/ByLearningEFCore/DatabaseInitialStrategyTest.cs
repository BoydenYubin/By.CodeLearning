using ByLearningEFCore.StrategyClass;
using Xunit;

namespace ByLearningEFCore
{
    public class DatabaseInitialStrategyTest
    {
        [Fact]
        public void EnsureCreatedTest()
        {
            DatabaseInitialStrategyContext context = new DatabaseInitialStrategyContext();
            //下面这个方法同然可以在生成迁移文件后，更新数据库
            //context.Database.Migrate();
        }

        [Fact]
        public void LoggerTest()
        {
            //日志打印在debug窗口
            LoggerDbContext context = new LoggerDbContext();
            context.Loggers.Add(new LoggerClass() { Name = "test" });
            context.SaveChanges();
            /*  optionsBuilder.EnableSensitiveDataLogging();
             Microsoft.EntityFrameworkCore.Database.Command: Information: Executed DbCommand (56ms) [Parameters=[@p0='test' (Size = 4000)], CommandType='Text', CommandTimeout='30']
             INSERT INTO `Loggers` (`Name`)
             VALUES (@p0);
             SELECT `ID`
             FROM `Loggers`
             WHERE ROW_COUNT() = 1 AND `ID` = LAST_INSERT_ID();
             */
            /*  不使用optionsBuilder.EnableSensitiveDataLogging();
             Microsoft.EntityFrameworkCore.Database.Command: Information: Executed DbCommand (56ms) [Parameters=[@p0=？ (Size = 4000)], CommandType='Text', CommandTimeout='30']
             INSERT INTO `Loggers` (`Name`)
             VALUES (@p0);
             SELECT `ID`
             FROM `Loggers`
             WHERE ROW_COUNT() = 1 AND `ID` = LAST_INSERT_ID();
             */
            context.Loggers.Add(new LoggerClass() { Name = "jackey" });
            context.SaveChanges();
        }
    }
}
