using System;
using System.Runtime.CompilerServices;
using System.Threading;

namespace ByLearningThread
{
    /// <summary>
    /// 特性测试
    /// </summary>
    public class MethodImplAttributeTest : ITestWork
    {
        private int id = 0;
        public void Run()
        {
            for (int i = 0; i < 5; i++)
            {
                Thread t = new Thread(ThreadProc);
                t.Name = "Thread_" + i.ToString();
                t.Start();
            }
            Thread.Sleep(1000);
            Console.WriteLine("Final Id is {0}", id);
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        private void ThreadProc()
        {
            string name = Thread.CurrentThread.Name;
            Console.WriteLine(name + "Enter the method");
            id++;
            Thread.Sleep(1000);
        }
    }
}
