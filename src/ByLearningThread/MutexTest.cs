using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ByLearningThread
{
    public class MutexTest : TestWork
    {
        public void FirstSample()
        {
            Mutex mut = new Mutex();
            int numIterations = 1;
            int numThreads = 3;
            // This method represents a resource that must be synchronized
            // so that only one thread at a time can enter.
            Action useResource = () =>
            {
                // Wait until it is safe to enter.
                Console.WriteLine("{0} is requesting the mutex",
                                  Thread.CurrentThread.Name);
                mut.WaitOne();
                //可以使用WaitOne加时间进行等待，如果等待成功返回true
                //否则返回false
                //if (mut.WaitOne(1000)) { }

                Console.WriteLine("{0} has entered the protected area",
                                  Thread.CurrentThread.Name);

                // Place code to access non-reentrant resources here.

                // Simulate some work.
                Thread.Sleep(500);

                Console.WriteLine("{0} is leaving the protected area",
                    Thread.CurrentThread.Name);

                // Release the Mutex.
                mut.ReleaseMutex();
                Console.WriteLine("{0} has released the mutex",
                    Thread.CurrentThread.Name);
            };
            // Create the threads that will use the protected resource.
            for (int i = 0; i < numThreads; i++)
            {
                Thread newThread = new Thread(new ThreadStart(() =>
                {
                    for (int i = 0; i < numIterations; i++)
                    {
                        useResource();
                    }
                }));
                newThread.Name = String.Format("Thread{0}", i + 1);
                newThread.Start();
            }

            // The main thread exits, but the application continues to
            // run until all foreground threads have exited.
        }
        public void SecondSample()
        {
            //Mutex可为跨进程使用
            bool result = false;
            //首次创建，out createNew为true
            Mutex mutexA = new Mutex(true, "test", out result);
            //再次创建，out createNew为false
            Mutex mutexB = new Mutex(true, "test", out result);
            bool[] preData = new bool[8];
            Parallel.For(0, 8, i =>
            {
                Mutex mutex = new Mutex(true, "reTest", out preData[i]);
            });
            if (preData.Where(b => b == true).Count() == 1)
            {
                Console.WriteLine("Only one mutex can create successfully!!");
            }
            else
            {
                Console.WriteLine("Got the wrong answer");
            }
        }
    }
}
