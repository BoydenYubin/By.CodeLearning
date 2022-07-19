using ByLearningORM.Util;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ByLearningEFCore.CreateModel
{
    public class Ownner
    {
        public int ID { get; set; }
        public string Name { get; set; }
    }

    public class Properties
    {
        public int PropertiesID { get; set;}
        public string Name { get; set; }
        public int OwnerID { get; set; }
    }
    public class OneToOneContext : DbContext
    {
        public DbSet<Ownner> Ownners { get; set; }
        public DbSet<Properties> Properties { get; set; }
        public OneToOneContext()
        {
            //不使用迁移文件，直接使用EnsureCreated
            Database.Migrate();
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
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
