using Autofac;
using Autofac.Extras.DynamicProxy;
using Castle.DynamicProxy;
using Xunit;

namespace ByLearningAutoFac.Autofac.Extras.DynamicProxyTest
{
    public class AdvancedUseTest
    {
        [Fact]
        public void DynamaticProxyTest()
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<CatInterceptpr>();
            builder.RegisterType<Cat>().As<ICat>();
            builder.RegisterType<CatOwner>().InterceptedBy(typeof(CatInterceptpr))
                .EnableClassInterceptors(ProxyGenerationOptions.Default, typeof(ICat));
            var container = builder.Build();
            var cat = container.Resolve<CatOwner>();
            cat.GetType().GetMethod("Eat").Invoke(cat, null);//因为我们的代理类添加了ICat接口，所以我们可以通过反射获取代理类的Eat方法来执行
        }
    }
    public class CatInterceptpr : IInterceptor
    {
        private readonly ICat _cat;
        public CatInterceptpr(ICat cat)
        {
            this._cat = cat;
        }
        public void Intercept(IInvocation invocation)
        {
            invocation.Method.Invoke(this._cat, invocation.Arguments);
        }
    }
    /// <summary>
    /// 为一个空类增加方法
    /// </summary>
    public class CatOwner { }
    public interface ICat
    {
        void Eat();
    }
    public class Cat : ICat
    {
        public void Eat() { }
    }
}
