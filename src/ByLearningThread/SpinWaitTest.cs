using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace ByLearningThread
{
    /// <summary>
    /// 自旋锁
    /// </summary>
    public class SpinWaitTest : TestWork
    {
        public void FirstSample()
        {
            bool someBoolean = false;
            int numYields = 0;

            // First task: SpinWait until someBoolean is set to true
            Task t1 = Task.Factory.StartNew(() =>
            {
                SpinWait sw = new SpinWait();
                while (!someBoolean)
                {
                    // The NextSpinWillYield property returns true if
                    // calling sw.SpinOnce() will result in yielding the
                    // processor instead of simply spinning.
                    if (sw.NextSpinWillYield) numYields++;
                    sw.SpinOnce();
                }

                // As of .NET Framework 4: After some initial spinning, SpinWait.SpinOnce() will yield every time.
                Console.WriteLine("SpinWait called {0} times, yielded {1} times", sw.Count, numYields);
            });

            // Second task: Wait 100ms, then set someBoolean to true
            Task t2 = Task.Factory.StartNew(() =>
            {
                Thread.Sleep(100);
                someBoolean = true;
            });

            // Wait for tasks to complete
            Task.WaitAll(t1, t2);
        }

        public void SecondSample()
        {
            bool someBoolean = false;
            Task t1 = Task.Factory.StartNew(() =>
            {
                Stopwatch swa = new Stopwatch();
                swa.Start();
                //如果在等待时间内自旋完成则返回true，
                //否则返回false
                //不带时间参数，则会一直自旋
                var res = SpinWait.SpinUntil(() => someBoolean, 150);
                swa.Stop();
                Console.WriteLine($"Result:{res},Spin time:{swa.ElapsedMilliseconds}ms");
            });
            Task t2 = Task.Factory.StartNew(() =>
            {
                Thread.Sleep(100);
                someBoolean = true;
            });
            Task.WaitAll(t1, t2);
        }
    }
}
