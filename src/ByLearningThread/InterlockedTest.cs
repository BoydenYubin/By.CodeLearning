using System;
using System.Linq;
using System.Threading;

namespace ByLearningThread
{
    public class InterlockedTest : TestWork
    {
        public void InterlockedAsLockSample()
        {
            int source = 0;
            int maxThread = 5;
            Action UseSource = () =>
            {
                //相当于加锁操作
                if (Interlocked.Exchange(ref source, 1) == 0)
                {
                    Console.WriteLine("{0} acquired the lock", Thread.CurrentThread.Name);
                    Thread.Sleep(new Random().Next(300, 800));
                    //相当于释放锁操作
                    Console.WriteLine("{0} released the lock", Thread.CurrentThread.Name);
                    Interlocked.Exchange(ref source, 0);
                }
            };
            for (int i = 0; i < maxThread; i++)
            {
                Thread thread = new Thread(new ThreadStart(UseSource));
                thread.Name = $"thread:{i}";
                Thread.Sleep(new Random().Next(300, 800));
                thread.Start();
            }
            Console.ReadLine();
        }
    }
}
