using Shouldly;
using System;
using System.Collections;
using System.Threading;
using Xunit;
using System.Linq;
using System.Collections.Concurrent;

namespace ByLearningBaseCode
{
    internal class MyClass
    {
        //用于测试构造函数被调用了多少次，以及各对象的HashCode
        public static ConcurrentQueue<int> List = new ConcurrentQueue<int>();
        public MyClass()
        {
            List.Enqueue(this.GetHashCode());
        }
    }
    public class LazyClassTest
    {
        [Fact]
        public void LazyThreadSafetyMode_ExecutionAndPublication_ShouldOnlyOne()
        {
            Lazy<MyClass> lazyObj = new Lazy<MyClass>(() => { return new MyClass(); }, LazyThreadSafetyMode.ExecutionAndPublication);
            //用于记录实际创建的对象的HashCode
            ConcurrentQueue<int> result = new ConcurrentQueue<int>();
            for (int i = 0; i < 10; i++)
            {
                Thread th = new Thread(() =>
                {
                    result.Enqueue(lazyObj.Value.GetHashCode());
                });
                th.Start();
            }
            Thread.Sleep(100);
            result.Distinct().Count().ShouldBe(1);
            MyClass.List.ToArray().Count().ShouldBe(1);
        }
        [Fact]
        public void LazyThreadSafetyMode_PublicationOnly_ShouldUseOne()
        {
            Lazy<MyClass> lazyObj = new Lazy<MyClass>(() => { return new MyClass(); }, LazyThreadSafetyMode.PublicationOnly);
            //用于记录实际创建的对象的HashCode
            ConcurrentQueue<int> result = new ConcurrentQueue<int>();
            for (int i = 0; i < 10; i++)
            {
                Thread th = new Thread(() =>
                {
                    result.Enqueue(lazyObj.Value.GetHashCode());
                });
                th.Start();
            }
            Thread.Sleep(100);
            result.Distinct().Count().ShouldBe(1);
            MyClass.List.ToArray().Distinct().Count().ShouldBeGreaterThan(1);
        }
        [Fact]
        public void LazyThreadSafetyMode_None_ShouldThrowException()
        {
            Lazy<MyClass> lazyObj = new Lazy<MyClass>(() => { return new MyClass(); }, LazyThreadSafetyMode.None);
            //用于记录实际创建的对象的HashCode
            ConcurrentQueue<int> result = new ConcurrentQueue<int>();
            for (int i = 0; i < 10; i++)
            {
                Thread th = new Thread(() =>
                {
                    try
                    {
                        result.Enqueue(lazyObj.Value.GetHashCode());
                    }
                    catch (InvalidOperationException ex)
                    {
                        ex.Message.ShouldBe("ValueFactory attempted to access the Value property of this instance.");
                    }
                });
                th.Start();
            }
        }
    }
}
