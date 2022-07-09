using System;
using System.Threading;
using System.Threading.Tasks;

namespace ByLearningThread
{
    /// <summary>
    /// 线程并行测试
    /// </summary>
    public class ParallelTest : ITestWork
    {
        private int num = 0;
        public void Run()
        {
            ParallelFor();
            ParallelForWithState();
            ParallelForeachTest();
        }

        private void ParallelFor()
        {
            Console.WriteLine("Parallel.For() Simple Use Test Start!!!");
            Parallel.For(0, 10, i =>
            {
                Console.WriteLine($"第{i}个，数字:{num}");
            });
            Thread.Sleep(1000);
        }

        private void ParallelForWithState()
        {
            Console.WriteLine("Parallel.For() Simple With State Test Start!!!");
            Parallel.For(0, 10, (i, state) =>
            {
                if (i > 3)
                    state.Break();
                Console.WriteLine($"第{i}个，数字:{num}");
            });
            Thread.Sleep(1000);
        }

        private void ParallelForeachTest()
        {
            Console.WriteLine("Parallel.Foreach() Simple With State Test Start!!!");
            string[] list = new string[5] { "A", "B", "C", "D", "E" };
            Parallel.ForEach<string>(list, str =>
            {
                Console.WriteLine($"字符:{str}");
            });
            Thread.Sleep(1000);
        }
    }
}
