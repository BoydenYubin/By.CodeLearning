using System;
using System.Threading;
using System.Threading.Tasks;

namespace ByLearningThread
{
    //信号量Semaphore图解释
    //Semaphore设置初始最大计数为3，初始值设为0
    //未释放的情况下，各线程均不允许进入
    //   Release                                   Release
    //      -------------------------------------------
    // G D A|                                         |  
    //      -------------------------------------------
    // E B  |                                         |
    //      -------------------------------------------
    // F C  |                                         |
    //      -------------------------------------------
    //通过Release(3)释放所有通道，信号量数量为0，此时A B C三个线程进入信号量
    //   Release                                   Release
    //      -------------------------------------------
    // G D  |              A                          |  
    //      -------------------------------------------
    // E    |             B                           |
    //      -------------------------------------------
    // F    |                     C                   |
    //      -------------------------------------------
    //此时信号量为3，D E F线程无法进入信号量
    //当A线程结束，并通过Release释放后，信号量变为2,
    //   Release                                   Release
    //      -------------------------------------------
    // G    |              D                          |  A
    //      -------------------------------------------
    // E    |                                B        |
    //      -------------------------------------------
    // F    |                                      C  |
    //      -------------------------------------------
    //此时D线程被允许进入
    public class SemaphoreSlimTest : ITestWork
    {
        private static SemaphoreSlim semaphore;
        // A padding interval to make the output more orderly.
        private static int padding;
        public void Run()
        {
            // Create the semaphore.
            semaphore = new SemaphoreSlim(0, 3);
            Console.WriteLine("{0} tasks can enter the semaphore.",
                              semaphore.CurrentCount);
            Task[] tasks = new Task[5];
            // Create and start five numbered tasks.
            for (int i = 0; i <= 4; i++)
            {
                tasks[i] = Task.Run(() =>
                {
                    // Each task begins by requesting the semaphore.
                    Console.WriteLine("Task {0} begins and waits for the semaphore.",
                                      Task.CurrentId);
                    int semaphoreCount;
                    semaphore.Wait();
                    try
                    {
                        Interlocked.Add(ref padding, 100);
                        Console.WriteLine("Task {0} enters the semaphore.", Task.CurrentId);
                        // The task just sleeps for 1+ seconds.
                        Thread.Sleep(1000 + padding);
                    }
                    finally
                    {
                        semaphoreCount = semaphore.Release();
                    }
                    Console.WriteLine("Task {0} releases the semaphore; previous count: {1}.",
                                      Task.CurrentId, semaphoreCount);
                });
            }
            // Wait for half a second, to allow all the tasks to start and block.
            Thread.Sleep(500);
            // Restore the semaphore count to its maximum value.
            Console.Write("Main thread calls Release(3) --> ");
            semaphore.Release(3);
            Console.WriteLine("{0} tasks can enter the semaphore.",
                              semaphore.CurrentCount);
            // Main thread waits for the tasks to complete.
            Task.WaitAll(tasks);
            Console.WriteLine("Main thread exits.");
        }
    }
}
