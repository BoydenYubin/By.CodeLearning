using Autofac;
using Autofac.Core;
using Shouldly;
using Xunit;

namespace ByLearningAutoFac
{
    public class InstanceClass { }
    public class InstanceLifetimeScopeTest
    {
        private ContainerBuilder builder;
        public InstanceLifetimeScopeTest()
        {
            builder = new ContainerBuilder();
        }
        [Fact]
        public void InstancePerDependencyTest()
        {
            builder.RegisterType<InstanceClass>().InstancePerDependency();
            var container = builder.Build();
            using (var scope = container.BeginLifetimeScope())
            {
                int hashcode = 0;
                for (var i = 0; i < 20; i++)
                {
                    var instance = scope.Resolve<InstanceClass>();
                    var tmp = instance.GetHashCode();
                    hashcode.ShouldNotBeSameAs(tmp);
                    hashcode = tmp;
                }
            }
        }
        [Fact]
        public void SingleInstanceTest()
        {
            builder.RegisterType<InstanceClass>().SingleInstance();
            var container = builder.Build();
            using (var scope = container.BeginLifetimeScope())
            {
                var instance = scope.Resolve<InstanceClass>();
                int hashcode = instance.GetHashCode();
                for (var i = 0; i < 20; i++)
                {
                    var tmp = scope.Resolve<InstanceClass>();
                    hashcode = tmp.GetHashCode();
                }
            }
        }
        [Fact]
        public void InstancePerLifetimeScopeTest()
        {
            builder.RegisterType<InstanceClass>().InstancePerLifetimeScope();
            var container = builder.Build();
            int hashcode = 0;
            using (var scope = container.BeginLifetimeScope())
            {
                var instance = scope.Resolve<InstanceClass>();
                hashcode = instance.GetHashCode();
                for (var i = 0; i < 20; i++)
                {
                    var tmp = scope.Resolve<InstanceClass>();
                    hashcode = tmp.GetHashCode();
                }
            }
            using (var scope = container.BeginLifetimeScope())
            {
                var instance = scope.Resolve<InstanceClass>();
                var tmp = instance.GetHashCode();
                hashcode.ShouldNotBeSameAs(tmp);
            }
        }

        [Fact]
        public void InstancePerMatchingLifetimeScopeTest()
        {
            builder.RegisterType<InstanceClass>().InstancePerMatchingLifetimeScope("lifetime");
            var container = builder.Build();
            // Create the lifetime scope using the tag.
            using (var scope1 = container.BeginLifetimeScope("lifetime"))
            {
                var instance1 = scope1.Resolve<InstanceClass>();
                using (var scope2 = scope1.BeginLifetimeScope())
                {
                    instance1.ShouldBeSameAs(scope2.Resolve<InstanceClass>());
                }
            }
            // Create another lifetime scope using the tag.
            using (var scope3 = container.BeginLifetimeScope("lifetime"))
            {
                //instance2 will be DIFFERENT than the instance resolved in the
                // earlier tagged lifetime scope.
                var instance2 = scope3.Resolve<InstanceClass>();
            }
            // This throws an exception because this scope doesn't
            // have the expected tag and neither does any parent scope!
            using (var noTagScope = container.BeginLifetimeScope())
            {
                ShouldThrowExtensions.ShouldThrow<DependencyResolutionException>(() =>
                {
                    var fail = noTagScope.Resolve<InstanceClass>();
                });
            }
        }
    }
}
