using System;
using System.Linq;

namespace ByLearningQuartz
{
    public interface ITestWork
    {
        void RunAsync();
    }
    /// <summary>
    /// 当有多个测试案例时，请继承该抽象类
    /// 多次测试方案采用反射，含Sample字样的方法，依次执行
    /// </summary>
    public abstract class TestWork : ITestWork
    {
        public void RunAsync()
        {
            var methods = this.GetType().GetMethods().Where(m => m.Name.Contains("Sample"));
            var className = this.GetType().Name;
            foreach (var method in methods)
            {
                Console.WriteLine($"{className}+{method.Name} Start");
                method.Invoke(this, null);
                Console.WriteLine($"{className}+{method.Name} End");
                Console.WriteLine("-- -- -- -- -- -- -- --");
            }
        }
    }
}
