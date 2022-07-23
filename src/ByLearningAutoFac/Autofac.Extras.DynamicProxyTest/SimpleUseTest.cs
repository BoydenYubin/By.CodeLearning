using Autofac;
using Autofac.Extras.DynamicProxy;
using Castle.DynamicProxy;
using Xunit;

namespace ByLearningAutoFac.Autofac.Extras.DynamicProxyTest
{
    public class SimpleUseTest
    {
        [Fact]
        public void EnableInterfaceInterceptorsTest()
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<TestInterceptor>();
            //EnableInterfaceInterceptors，将会对接口方法实现拦截
            builder.RegisterType<Foo>().As<IFoo>().InterceptedBy(typeof(TestInterceptor)).EnableInterfaceInterceptors();
            var container = builder.Build();
            //生成IFooProxy接口对象
            var foo = container.Resolve<IFoo>(); 
            foo.AddValue();
            //无法强制转换为Foo对象，将会抛出异常
            //((Foo)foo).DecreaseValue();       
        }
        [Fact]
        public void EnableClassInterceptorsTest()
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<TestInterceptor>();
            //EnableClassInterceptors，将会对类方法实现拦截，并且类的方法需要使用virtual关键字
            builder.RegisterType<Foo>().As<IFoo>().AsSelf().InterceptedBy(typeof(TestInterceptor)).EnableClassInterceptors();
            var container = builder.Build();
            //生成FooProxy类
            var foo = container.Resolve<Foo>();
            //接口方法若不带virtual关键字将不会走拦截器
            foo.AddValue();
            //类方法带virtual关键字将会走拦截器
            foo.DecreaseValue();
        }
    }
    public class TestInterceptor : IInterceptor
    {
        public void Intercept(IInvocation invocation)
        {
            invocation.Proceed();
        }
    }
    public interface IFoo
    {
        void AddValue();
    }
    public class Foo : IFoo
    {
        public virtual void AddValue()
        {

        }
        public virtual void DecreaseValue()
        {

        }
    }
}
