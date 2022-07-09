using System;
using System.Threading;

namespace ByLearningThread
{
    /// <summary>
    ///            WaitOne                                  
    ///       -------------------------------------------
    ///             A |                                          
    ///             B |                                          
    ///             C |                                          
    ///       -------------------------------------------
    /// 线程使用WaitOne将线程通道堵塞，所有的线程A，B，C被堵塞在通道里
    ///              Set                               
    ///       -------------------------------------------
    ///                   A                                            
    ///                   B                                            
    ///                   C                                            
    ///       -------------------------------------------
    /// 如果不调用Reset方法则该通道可继续同行
    ///              Set                               
    ///       -------------------------------------------
    ///                                 A                                
    ///                 D               B                                
    ///                                 C                                
    ///       -------------------------------------------
    /// 如果调用Reset方法则该相当于继续封闭通道，等待Set方法将通道打开   
    ///            Reset                                
    ///       -------------------------------------------
    ///               |                               A               
    ///          E    |               D               B               
    ///               |                               C               
    ///       -------------------------------------------
    ///</summary>
    public class ManualResetEventTest : ITestWork
    {
        public ManualResetEvent mre = new ManualResetEvent(false);
        public void Run()
        {
            Console.WriteLine("\nStart 3 named threads that block on a ManualResetEvent:\n");

            for (int i = 0; i <= 2; i++)
            {
                Thread t = new Thread(ThreadProc);
                t.Name = "Thread_" + i;
                t.Start();
            }

            Thread.Sleep(500);
            Console.WriteLine("\nWhen all three threads have started, press Enter to call Set()" +
                              "\nto release all the threads.\n");
            Console.ReadLine();

            mre.Set();

            Thread.Sleep(500);
            Console.WriteLine("\nWhen a ManualResetEvent is signaled, threads that call WaitOne()" +
                              "\ndo not block. Press Enter to show this.\n");
            Console.ReadLine();

            for (int i = 3; i <= 4; i++)
            {
                Thread t = new Thread(ThreadProc);
                t.Name = "Thread_" + i;
                t.Start();
            }

            Thread.Sleep(500);
            Console.WriteLine("\nPress Enter to call Reset(), so that threads once again block" +
                              "\nwhen they call WaitOne().\n");
            Console.ReadLine();

            mre.Reset();

            // Start a thread that waits on the ManualResetEvent.
            Thread t5 = new Thread(ThreadProc);
            t5.Name = "Thread_5";
            t5.Start();

            Thread.Sleep(500);
            Console.WriteLine("\nPress Enter to call Set() and conclude the demo.");
            Console.ReadLine();

            mre.Set();

            // If you run this example in Visual Studio, uncomment the following line:
            //Console.ReadLine();
        }

        private void ThreadProc()
        {
            string name = Thread.CurrentThread.Name;

            Console.WriteLine(name + " starts and calls mre.WaitOne()");

            mre.WaitOne(5000);

            Console.WriteLine(name + " ends.");
        }
    }
}
