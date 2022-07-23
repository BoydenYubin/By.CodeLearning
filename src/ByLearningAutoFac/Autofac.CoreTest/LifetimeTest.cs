using Autofac;
using Autofac.Core;
using Shouldly;
using Xunit;

namespace ByLearningAutoFac
{
    public interface IServices { }
    public class FooServices : IServices { }
    /// <summary>
    /// 生命周期
    /// </summary>
    public class LifetimeTest
    {
        [Fact]
        public void SimpleUseTest()
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<FooServices>().As<IServices>();
            var container = builder.Build();
            using(var scope = container.BeginLifetimeScope())
            {
                var services = scope.Resolve<IServices>();
                services.GetType().ShouldBe(typeof(FooServices));
                using(var nestscope = scope.BeginLifetimeScope())
                {
                    var nestservices = nestscope.Resolve<IServices>();
                    nestservices.GetType().ShouldBe(typeof(FooServices));
                }
            }
        }
        [Fact]
        public void LifetimeWithTagTest()
        {
            // Register your transaction-level shared component
            // as InstancePerMatchingLifetimeScope and give it
            // a "known tag" that you'll use when starting new
            // transactions.
            var builder = new ContainerBuilder();
            builder.RegisterType<FooServices>()
                   .As<IServices>()
                   .InstancePerMatchingLifetimeScope("transaction");
            var container = builder.Build();
            using (var scope = container.BeginLifetimeScope("transaction"))
            {
                var services = scope.Resolve<IServices>();
                services.GetType().ShouldBe(typeof(FooServices));
            }
            using (var scope = container.BeginLifetimeScope())
            {
                ShouldThrowExtensions.ShouldThrow<DependencyResolutionException>(() =>
                {
                    var services = scope.Resolve<IServices>();
                    services.GetType().ShouldBe(typeof(FooServices));
                });
            }
        }
        [Fact]
        public void AddBuilderInScopeTest()
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<FooServices>().As<IServices>();
            var container = builder.Build();
            using (var scope = container.BeginLifetimeScope(builder => 
            {
                //builder can register sth in here
            }))
            {
                // The additional registrations will be available
                // only in this lifetime scope.
            }
        }
    }
}
