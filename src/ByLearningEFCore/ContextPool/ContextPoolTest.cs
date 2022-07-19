using ByLearningEFCore.CreateModel;
using ByLearningORM.Util;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace ByLearningEFCore.ContextPool
{
    public class ContextPoolTest
    {
        [Fact]
        public void ContextPoolSimpleUseTest()
        {
            var services = new ServiceCollection();
            services.AddDbContextPool<PoolDbContext>(builder => builder.UseMySql(GetConfig.GetConnectionString()), poolSize: 64);
            var provider = services.BuildServiceProvider();
        }
    }
}
