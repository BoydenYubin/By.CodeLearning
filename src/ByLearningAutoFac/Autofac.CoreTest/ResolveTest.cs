using Autofac;
using Autofac.Features.OwnedInstances;
using Shouldly;
using System;
using System.Collections.Generic;
using Xunit;

namespace ByLearningAutoFac.Autofac.Core
{
    /// <summary>
    /// https://autofaccn.readthedocs.io/zh/latest/resolve/
    /// 
    /// </summary>
    public class ResolveTest
    {
        private ContainerBuilder builder;
        public ResolveTest()
        {
            builder = new ContainerBuilder();
        }
        [Fact]
        public void ResolveOptionalTest()
        {
            var container = builder.Build();
            var service = container.ResolveOptional<IBar>();
            service.ShouldBeNull();
        }
        [Fact]
        public void TryResolveTest()
        {
            var container = builder.Build();
            using (var scope = container.BeginLifetimeScope())
            {
                var result = scope.TryResolve<IBar>(out IBar bar);
                result.ShouldBeFalse();
                bar.ShouldBeNull();
            }
        }
        #region 直接依赖
        //注册 A 和 B 组件, 然后解析:

        //var builder = new ContainerBuilder();
        //builder.RegisterType<A>();
        //builder.RegisterType<B>();
        //var container = builder.Build();

        //using(var scope = container.BeginLifetimeScope())
        //{
        //  // B is automatically injected into A.
        //  var a = scope.Resolve<A>();
        //}
        #endregion

        [Fact]
        public void LazyClassResolveTest()
        {
            builder.RegisterType<NeedLazyClass>().AsSelf();
            builder.RegisterType<LazyClass>().AsSelf();
            var container = builder.Build();
            var instance = container.Resolve<NeedLazyClass>();
            instance.AddValue();
            instance.Value.ShouldBe(29);
        }
        [Fact]
        public void OwnedClassResolveTest()
        {
            builder.RegisterType<NeedOwnedClass>().AsSelf();
            builder.RegisterType<OwnedClass>().AsSelf();
            var container = builder.Build();
            var instance = container.Resolve<NeedOwnedClass>();
            instance.AddValue();
            instance.Value.ShouldBe(29);
        }
        [Fact]
        public void FuncClassResolveTest()
        {
            builder.RegisterType<NeedFuncClass>().AsSelf();
            builder.Register<Func<FuncClass>>(cfg => () => { var ret = new FuncClass(); ret.Value++; return ret; });
            var container = builder.Build();
            var instance = container.Resolve<NeedFuncClass>();
            instance.AddValue();
            instance.Value.ShouldBe(28);
        }
        [Fact]
        public void IEnumerableClassTest()
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<FirstHandler>().As<IMessageHandler>();
            builder.RegisterType<SecondHandler>().As<IMessageHandler>();
            builder.RegisterType<ThirdHandler>().As<IMessageHandler>();
            builder.RegisterType<MessageProcessor>();
            var container = builder.Build();

            using (var scope = container.BeginLifetimeScope())
            {
                // When processor is resolved, it'll have all of the
                // registered handlers passed in to the constructor.
                var processor = scope.Resolve<MessageProcessor>();
                processor.ProcessMessage(12);
                processor.Value.ShouldBe(15);
            }
        }
    }
    public class NeedLazyClass
    {
        private Lazy<LazyClass> LazyClass;
        public int Value = 27;
        public NeedLazyClass(Lazy<LazyClass> lazy)
        {
            this.LazyClass = lazy;
        }
        public void AddValue()
        {
            this.Value = this.LazyClass.Value.AddTwo(this.Value);
        }
    }
    public class LazyClass
    {
        public int AddTwo(int num)
        {
            return num + 2;
        }
    }
    public class NeedOwnedClass
    {
        private Owned<OwnedClass> Owned;
        public int Value = 27;
        public NeedOwnedClass(Owned<OwnedClass> owned)
        {
            this.Owned = owned;
        }
        public void AddValue()
        {
            this.Value = this.Owned.Value.AddTwo(this.Value);
            // Here OwnedClass is no longer needed, so
            // it is released
            this.Owned.Dispose();
        }
    }
    public class OwnedClass
    {
        public int AddTwo(int value) => value + 2;
    }
    public class NeedFuncClass
    {
        private Func<FuncClass> Func;
        public int Value;
        public NeedFuncClass(Func<FuncClass> func)
        {
            this.Func = func;
        }
        public void AddValue()
        {
            this.Value = this.Func().Value;
        }
    }
    public class FuncClass
    {
        public int Value = 27;
    }

    public interface IMessageHandler
    {
        int HandleMessage(int value);
    }
    public class FirstHandler : IMessageHandler
    {
        public int HandleMessage(int value)
        {
            return ++value;
        }
    }
    public class SecondHandler : IMessageHandler
    {
        public int HandleMessage(int value)
        {
            return ++value;
        }
    }
    public class ThirdHandler : IMessageHandler
    {
        public int HandleMessage(int value)
        {
            return ++value;
        }
    }
    public class MessageProcessor
    {
        private IEnumerable<IMessageHandler> _handlers;
        public int Value;
        public MessageProcessor(IEnumerable<IMessageHandler> handlers)
        {
            this._handlers = handlers;
        }

        public void ProcessMessage(int value)
        {
            this.Value = value;
            foreach (var handler in this._handlers)
            {
                this.Value = handler.HandleMessage(this.Value);
            }
        }
    }
}
