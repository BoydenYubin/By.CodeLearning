using Autofac;
using Autofac.Core;
using Shouldly;
using Xunit;

namespace ByLearningAutoFac
{
    /// <summary>
    /// 注册概念
    /// </summary>
    public class RegisterTest
    {
        [Fact]
        public void SimpleUseTest()
        {
            // Create your builder.
            var builder = new ContainerBuilder();
            // Usually you're only interested in exposing the type
            // via its interface:
            builder.RegisterType<Foo>().As<IFoo>();
            var container = builder.Build();
            using (var scope = container.BeginLifetimeScope())
            {
                var foo = scope.Resolve<IFoo>();
                foo.GetType().ShouldBe(typeof(Foo));
            }
        }
        [Fact]
        public void UsingConstructorTest01()
        {
            var builder = new ContainerBuilder();
            // 直接使用UsingConstructor方法注册,将会舍弃其它构造函数
            builder.RegisterType<Foo>().UsingConstructor(typeof(IBar)).As<IFoo>();
            builder.RegisterType<Bar>().As<IBar>();
            var container = builder.Build();
            using (var scope = container.BeginLifetimeScope())
            {
                var foo = scope.Resolve<IFoo>();
                foo.Bar.Name.ShouldBe("boyden");
            }
        }
        [Fact]
        public void UsingConstructorTest02()
        {
            //不使用构造函数注册的情况下，容器会选择
            //能找到最对参数的那个构造函数进行构造
            //此例中由于只注册了Bar，因此会选择构造函数 Foo(IBar bar)
            var builder = new ContainerBuilder();
            builder.RegisterType<Foo>().UsingConstructor(typeof(IBar)).As<IFoo>();
            //builder.RegisterType<Foo>().UsingConstructor(typeof(IBar)).As<IFoo>();
            //builder.RegisterType<Baz>().As<IBaz>();
            //若采用UsingConstructor方法，即使注册了Baz也无法使用，同样为null
            builder.RegisterType<Bar>().As<IBar>();
            var container = builder.Build();
            using (var scope = container.BeginLifetimeScope())
            {
                var foo = scope.Resolve<IFoo>();
                foo.Bar.Name.ShouldBe("boyden");
                foo.Baz.ShouldBeNull();
            }
        }
        [Fact]
        public void UsingLambdaTest()
        {
            var builder = new ContainerBuilder();
            builder.Register(cfg => new Bar("test")).As<IBar>();
            var container = builder.Build();
            using (var scope = container.BeginLifetimeScope())
            {
                var bar = scope.Resolve<IBar>();
                bar.Name.ShouldBe("test");
            }
        }
        [Fact]
        public void ParamSelectedTest()
        {
            var builder = new ContainerBuilder();
            builder.Register<Card>((c, p) =>
            {
                var id = p.Named<int>("Id");
                if (id > 0) return new BalckCard();
                else return new WhiteCard();
            });
            var container = builder.Build();
            var blackCard = container.Resolve<Card>(new NamedParameter("Id", 12));
            blackCard.GetType().ShouldBe(typeof(BalckCard));
            var whiteCard = container.Resolve<Card>(new NamedParameter("Id", -12));
            whiteCard.GetType().ShouldBe(typeof(WhiteCard));
        }
        [Fact]
        public void GenericRegisterTest()
        {
            var builder = new ContainerBuilder();
            builder.RegisterGeneric(typeof(GenericClass<>))
                .As(typeof(IGeneric<>)).InstancePerLifetimeScope();
            var container = builder.Build();
            var stringRes = container.Resolve<IGeneric<string>>();
            stringRes.Tobject = "test";
            stringRes.Tobject.ShouldBe("test");
            var barRes = container.Resolve<IGeneric<Bar>>();
            barRes.Tobject = new Bar("test");
            barRes.Tobject.GetType().ShouldBe(typeof(Bar));
        }
        [Fact]
        public void RegisterWithConditionTest()
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<Bar>().As<IBar>();
            //只有Bar被注册
            builder.RegisterType<Baz>().As<IBaz>().IfNotRegistered(typeof(IBar));

            //此外还要考虑注册的顺序
            //这种情况下，Baz会被注册，因为检查前它不会发现IBaz
            //builder.RegisterType<Baz>().As<IBaz>().IfNotRegistered(typeof(IBar));
            //builder.RegisterType<Bar>().As<IBar>();
            //此外注意AsSelf和As接口的区别
            //下面这种情况就可以注册，因为检查的Bar是以接口形式注册的
            //builder.RegisterType<Bar>().As<IBar>();
            //builder.RegisterType<Baz>().As<IBaz>().IfNotRegistered(typeof(Bar));

            //OnlyIf的使用方法
            builder.RegisterType<Foo>().As<IFoo>().OnlyIf(reg =>
            reg.IsRegistered(new TypedService(typeof(IBar)))
            && reg.IsRegistered(new TypedService(typeof(IBaz)))); 
        }
    }
}
