using ByLearningORM.Util;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace ByLearningEFCore.CreateModel
{
    public class ShadowPropertyClass
    {
        public int ID { get; set; }
        public string Name { get; set; }
        //这里还将将包含一个不愿意暴露给外部的shadow属性
    }

    public class ShadowPropertyContext : DbContext 
    {
        public DbSet<ShadowPropertyClass> Shadows { get; set; }
        public ShadowPropertyContext()
        {
            Database.Migrate();
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySql(GetConfig.GetConnectionString());
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ShadowPropertyClass>().ToTable("shadowproperty");
            modelBuilder.Entity<ShadowPropertyClass>().HasIndex(b => b.ID);
            modelBuilder.Entity<ShadowPropertyClass>()
                .Property(b => b.Name).HasColumnType("varchar(40)");
            //这里还将将包含一个不愿意暴露给外部的shadow属性createtime
            modelBuilder.Entity<ShadowPropertyClass>().Property<DateTime>("createtime");
        }
    }
}
