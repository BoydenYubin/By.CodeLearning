using ByLearningORM.Util;
using Microsoft.EntityFrameworkCore;

namespace ByLearningEFCore.StrategyClass
{
    public class DatabaseInitialStrategyTable
    {
        // 第一次添加字段
        //--------------------------------
        //ID字样的会被当做主键，不区分大小写
        //其它约定为使用类名+Id，如DatabaseInitialStrategyTableId
        public int ID { get; set; } 
        //string类型默认生成longtext型的
        public string Name { get; set; }
        //--------------------------------
        //********************************
        // 第二次添加字段
        //--------------------------------
        public string AddStuff { get; set; }
        //--------------------------------
        //********************************
        // 第三次添加字段
        //--------------------------------
        public string ThirdStuff { get; set; }
        //--------------------------------
    }
    public class DatabaseInitialStrategyContext : DbContext
    {
        DbSet<DatabaseInitialStrategyTable> StrategyTable { get; set; }
        public DatabaseInitialStrategyContext()
        {
            //如果数据库不存在则创建
            //如果已存在则什么也不处理
            //上表中第二次添加的字段无法被加载进新表中
            //会报数据库已存在的错误，
            //Database.EnsureCreated();

            //更新数据库的两种方式
            //一
            //根据增加的第二次的内容使用程序包控制器控制台，添加迁移文件 add-migration
            //并使用updata-database创建数据库,不使用Database.Migrate(); 依然可以更新数据库
            //二
            //或者添加完Migration后不调用updata-database，也可以更新数据库
            //则使用Migrate则可以将第三次添加的字段添加进数据表中
            //同时会在数据库中创建一个__efmigrationshistory的表
            Database.Migrate();

            //这就是EnsureCreated和Migrate的区别，如果EnsureCreated后创立数据库后
            //使用Migrate，则会报数据库已存在的错误
            //因此不建议使用
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //使用mysql作为测试数据库
            optionsBuilder.UseMySql(GetConfig.GetConnectionString());
        }
    }
}