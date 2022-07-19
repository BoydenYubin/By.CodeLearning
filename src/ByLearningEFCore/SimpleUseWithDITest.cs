using ByLearningEFCore.CreateModel;
using ByLearningORM.Util;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace ByLearningEFCore
{
    public class SimpleUseWithDITest
    {
        private IServiceCollection services;
        private ServiceProvider provider;
        public SimpleUseWithDITest()
        {
            services = new ServiceCollection();
            services.AddDbContext<QueryDataContext>(builder =>
            {
                builder.UseMySql(GetConfig.GetConnectionString());
                var looger = LoggerFactory.Create(config =>
                {
                    config.AddDebug();
                });
                builder.UseLoggerFactory(looger);
                //以下语句将会记录参数值
                builder.EnableSensitiveDataLogging();
            });
            provider = services.BuildServiceProvider();
        }
        [Fact]
        public void SimpleUseToSeedDataTest()
        {
            using (var score = provider.CreateScope())
            {
                var context = score.ServiceProvider.GetRequiredService<QueryDataContext>();
                var db = context.Set<QueryData>();
                for (int i = 0; i < 10000; i++)
                {
                    db.Add(new QueryData() { Name = i, CreateTime = DateTime.Now });
                }
                context.SaveChanges();
            }
        }
        [Fact]
        public void DeleteByIdsTest()
        {
            using (var score = provider.CreateScope())
            {
                var context = score.ServiceProvider.GetRequiredService<QueryDataContext>();
                var db = context.Set<QueryData>();
                var list = new List<int> { 1, 2, 5 };
                db.Remove(db.Find(3));

                context.SaveChanges();

                db.Remove(db.Find(4));

                context.SaveChanges();
            }
        }
    }
}
