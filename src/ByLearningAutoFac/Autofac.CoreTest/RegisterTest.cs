using Autofac;
using Autofac.Core;
using Shouldly;
using Xunit;

namespace ByLearningAutoFac
{
    /// <summary>
    /// ע�����
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
            // ֱ��ʹ��UsingConstructor����ע��,���������������캯��
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
            //��ʹ�ù��캯��ע�������£�������ѡ��
            //���ҵ���Բ������Ǹ����캯�����й���
            //����������ֻע����Bar����˻�ѡ���캯�� Foo(IBar bar)
            var builder = new ContainerBuilder();
            builder.RegisterType<Foo>().UsingConstructor(typeof(IBar)).As<IFoo>();
            //builder.RegisterType<Foo>().UsingConstructor(typeof(IBar)).As<IFoo>();
            //builder.RegisterType<Baz>().As<IBaz>();
            //������UsingConstructor��������ʹע����BazҲ�޷�ʹ�ã�ͬ��Ϊnull
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
            //ֻ��Bar��ע��
            builder.RegisterType<Baz>().As<IBaz>().IfNotRegistered(typeof(IBar));

            //���⻹Ҫ����ע���˳��
            //��������£�Baz�ᱻע�ᣬ��Ϊ���ǰ�����ᷢ��IBaz
            //builder.RegisterType<Baz>().As<IBaz>().IfNotRegistered(typeof(IBar));
            //builder.RegisterType<Bar>().As<IBar>();
            //����ע��AsSelf��As�ӿڵ�����
            //������������Ϳ���ע�ᣬ��Ϊ����Bar���Խӿ���ʽע���
            //builder.RegisterType<Bar>().As<IBar>();
            //builder.RegisterType<Baz>().As<IBaz>().IfNotRegistered(typeof(Bar));

            //OnlyIf��ʹ�÷���
            builder.RegisterType<Foo>().As<IFoo>().OnlyIf(reg =>
            reg.IsRegistered(new TypedService(typeof(IBar)))
            && reg.IsRegistered(new TypedService(typeof(IBaz)))); 
        }
    }
}
