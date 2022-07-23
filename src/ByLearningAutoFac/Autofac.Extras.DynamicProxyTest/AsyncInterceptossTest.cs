using Autofac;
using Autofac.Extras.DynamicProxy;
using Castle.DynamicProxy;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace ByLearningAutoFac.Autofac.Extras.DynamicProxyTest
{
    public class AsyncInterceptossTest
    {
        private readonly ITestOutputHelper _output;

        public AsyncInterceptossTest(ITestOutputHelper output)
        {
            this._output = output;
        }
        [Fact]
        public async void ReturnOnlyTaskTest()
        {
            var builder = new ContainerBuilder();
            //builder.RegisterType<AsyncDeterminationInterceptor>();

            //EnableClassInterceptors，将会对类方法实现拦截，并且类的方法需要使用virtual关键字
            //builder.RegisterType<AsyncFoo>().As<IAsyncFoo>().AsSelf().InterceptedBy(typeof(AsyncTestInterceptor)).EnableClassInterceptors();

            builder.RegisterType<InOutValueAsyncInterceptor>();
            builder.RegisterInstance(this._output).As<ITestOutputHelper>();
            builder.RegisterType<ListLogger>().SingleInstance();
            builder.RegisterType<InOutValueInterceptor>();

            builder.RegisterType<AsyncFoo>().As<IAsyncFoo>().AsSelf()
                .InterceptedBy(typeof(InOutValueInterceptor)).EnableClassInterceptors();


            var container = builder.Build();
            //生成FooProxy类
            var foo = container.Resolve<AsyncFoo>();
            //调用同步方法
            var val = foo.SyncMethod(12, 15);
            Assert.Equal(27, val);

            //调用返回Task方法
            await foo.AddValue();

            //调用返回Task<result>方法
            var result = await foo.DecreaseValue();
        }


    }
    /// <summary>
    /// <see cref="AsyncDeterminationInterceptor"/>
    /// AsyncDeterminationInterceptor继承了IInterceptor
    /// 可作为被InterceptedBy方法使用的Type
    /// </summary>
    public class InOutValueInterceptor : AsyncDeterminationInterceptor
    {
        public InOutValueInterceptor(InOutValueAsyncInterceptor inOutValueAsync) : base(inOutValueAsync)
        {
        }
    }

    public class InOutValueAsyncInterceptor : IAsyncInterceptor
    {
        private readonly ListLogger _logger;

        public InOutValueAsyncInterceptor(ListLogger logger)
        {
            this._logger = logger;
        }
        public void InterceptAsynchronous(IInvocation invocation)
        {
            this._logger.Add("AsyncMethod Intercept Start");
            invocation.Proceed();
            var task = (Task)invocation.ReturnValue;
            task.ConfigureAwait(false);
            this._logger.Add("AsyncMethod Intercept End");
        }

        public async void InterceptAsynchronous<TResult>(IInvocation invocation)
        {
            this._logger.Add("AsyncResultMethod Intercept Start");
            invocation.Proceed();
            var task = (Task<TResult>)invocation.ReturnValue;
            TResult result = await task;
            //记录这个返回结果
            this._logger.Add($"async result:{result}");
            invocation.ReturnValue = result;
            this._logger.Add("AsyncResultMethod Intercept End");
        }

        public void InterceptSynchronous(IInvocation invocation)
        {
            this._logger.Add("SyncMethod Intercept Start");
            this._logger.Add($"入参：{invocation.Arguments[0]},{invocation.Arguments[1]}");
            invocation.Proceed();
            this._logger.Add("SyncMethod Intercept End");
        }
    }

    public interface IAsyncFoo
    {
        Task AddValue();
    }
    public class AsyncFoo : IAsyncFoo
    {
        private readonly ListLogger _logger;

        public AsyncFoo(ListLogger logger)
        {
            this._logger = logger;
        }
        /// <summary>
        /// 同步方法，带返回值
        /// </summary>
        public virtual int SyncMethod(int a, int b)
        {
            try
            {
                this._logger.Add("SyncMethod Start");
                return a + b;
            }
            catch { throw; }
            finally
            {
                this._logger.Add("SyncMethod End");
            }
        }
        /// <summary>
        /// 返回Task
        /// </summary>
        /// <returns></returns>
        public virtual async Task AddValue()
        {
            this._logger.Add("AsyncMethod Start");
            await Console.Out.WriteLineAsync($"当前线程：{Thread.CurrentThread.ManagedThreadId}");
            this._logger.Add("AsyncMethod End");
        }
        public virtual async Task<int> DecreaseValue()
        {
            this._logger.Add("AsyncResultMethod Start");
            await Task.Delay(10).ConfigureAwait(false);
            this._logger.Add("AsyncResultMethod End");
            return 5;
        }
    }
}
