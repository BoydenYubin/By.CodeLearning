using ByLearningORM.Util;
using Microsoft.EntityFrameworkCore;

namespace ByLearningEFCore.TestClass
{
    public class BackingFieldsClass
    {
        public BackingFieldsClass() { }
        public BackingFieldsClass(string url)
        {
            _url = url;
        }
        private string _url;
        public string Url => _url;
        public int ID { get; set; }
    }

    public class BackingFieldsContext : DbContext
    {
        public DbSet<BackingFieldsClass> BackingFields { get; set; }
        public BackingFieldsContext()
        {
            //不使用迁移文件，直接使用EnsureCreated
            Database.Migrate();
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySql(GetConfig.GetConnectionString());
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BackingFieldsClass>().ToTable("backingfields");
            modelBuilder.Entity<BackingFieldsClass>().HasIndex(b => b.ID);
            modelBuilder.Entity<BackingFieldsClass>()
                .Property(b => b.Url).HasColumnType("varchar(40)").HasField("_url");
        }
    }

}
