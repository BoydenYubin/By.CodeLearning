using System;
using System.Threading;

namespace ByLearningThread
{
    /// <summary>
    /// <seealso cref="ManualResetEventTest"/>
    /// 带有slim意味着CPU自旋，防止CPU调度耗时
    /// 但是自旋时间要短
    /// </summary>
    public class ManualResetEventSlimTest : ITestWork
    {
        private ManualResetEventSlim mres = new ManualResetEventSlim(false, 100);
        public void Run()
        {
            Thread thread = new Thread(ThreadProc);
            thread.Start();
            Console.ReadLine();
            mres.Set();
        }
        private void ThreadProc()
        {
            string name = Thread.CurrentThread.Name;
            Console.WriteLine(name + " starts and calls mre.WaitOne()");
            mres.Wait();
            Console.WriteLine(name + " ends.");
        }
    }
}
