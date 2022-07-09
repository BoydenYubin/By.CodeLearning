using System;
using System.Threading;
using System.Threading.Tasks;

namespace ByLearningThread
{
    /// <summary>
    /// Barier相当于设置了一个路障，多个线程必须同时到达时才能开发
    /// -----------------------------------------------------------
    ///                                 A            |
    ///                          B                   |
    ///                                        C     |
    ///                   D                          |
    /// -----------------------------------------------------------
    /// 假设A B C D四个线程达到barrier的点时间如上所示
    ///                                   barrier.SignalAndWait();
    /// -----------------------------------------------------------
    ///                                            A |
    ///                                            B |
    ///                                            C |
    ///                                            D |
    /// -----------------------------------------------------------
    /// 这一时刻同时到达，barrier"放行"，线程同时开放
    /// </summary>
    public class BarierTest : ITestWork
    {
        public void Run()
        {
            Barrier barrier = new Barrier(4, it => {
                Console.WriteLine("再次集结，友谊万岁，再次开跑");
            });
            string[] names = { "张三", "李四", "王五", "赵六" };
            Random random = new Random();
            foreach (string name in names)
            {
                Task.Run(() => {
                    Console.WriteLine($"{name}开始跑");
                    int t = random.Next(1, 10);
                    Thread.Sleep(t * 1000);
                    Console.WriteLine($"{name}用时{t}秒，跑到友谊集结点");
                    barrier.SignalAndWait();
                    Console.WriteLine($"友谊万岁，{name}重新开始跑");
                });
            }
            Console.ReadKey();
        }
    }
}
