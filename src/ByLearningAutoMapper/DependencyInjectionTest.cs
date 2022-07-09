using AutoMapper;
using ByLearningAutoMapper.IOC;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Xunit;

namespace ByLearningAutoMapper
{
    public class DependencyInjectionTest
    {
        private ServiceCollection _services;

        public DependencyInjectionTest()
        {
            _services = new ServiceCollection();
        }
        [Fact]
        public void AddProfileTest()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<ProfileClass>();
            });
            var mapper = new Mapper(config);
            var dst = mapper.Map<Destination>(new SourceClass { Name = "Boyden", Age = 28 });
            dst.DName.ShouldBe("Boyden");
            dst.DAge.ShouldBe(28);
        }
        [Fact]
        public void AddMapperTest()
        {
            _services.AddAutoMapper(typeof(ProfileClass));
            var app = _services.BuildServiceProvider();
            using(var scope = app.CreateScope())
            {
                var mapper = scope.ServiceProvider.GetRequiredService<IMapper>();
                var dst = mapper.Map<Destination>(new SourceClass { Name = "Boyden", Age = 28 });
                dst.DName.ShouldBe("Boyden");
                dst.DAge.ShouldBe(28);
            }
        }
    }
}