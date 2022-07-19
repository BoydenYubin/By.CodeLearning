using ByLearningORM.Util;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace ByLearningEFCore.CreateModel
{
    public class QueryData
    {
        public int ID { get; set; }
        public int Name { get; set; }
        public DateTime CreateTime { get; set; }
    }
    public class QueryDataContext : DbContext
    {
        public QueryDataContext(DbContextOptions<QueryDataContext> options)
            :base(options)
        {
            base.Database.EnsureCreated();
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<QueryData>().ToTable("querydata");
        }
    }

    public class QueryDataContextFactory : IDesignTimeDbContextFactory<QueryDataContext>
    {
        public QueryDataContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<QueryDataContext>();
            optionsBuilder.UseMySql(GetConfig.GetConnectionString());
            return new QueryDataContext(optionsBuilder.Options);
        }
    }

}
