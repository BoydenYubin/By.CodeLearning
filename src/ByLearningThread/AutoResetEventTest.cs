using System;
using System.Threading;

namespace ByLearningThread
{
    /// <summary>
    /// <seealso cref="ManualResetEventTest"/>
    /// AutoReset代表无需手动调用ReSet函数
    /// </summary>
    public class AutoResetEventTest : ITestWork
    {
        private AutoResetEvent producer = new AutoResetEvent(false);
        private AutoResetEvent consumer = new AutoResetEvent(false);
        public void Run()
        {
            Thread produceThread = new Thread(ProduceProduct);
            Thread consumeThread = new Thread(ConsumeProduct);
            produceThread.Start();
            Thread.Sleep(10);
            consumeThread.Start();
        }
        private void ProduceProduct()
        {
            while (true)
            {
                Console.WriteLine("Now produce a product now");
                consumer.Set();
                producer.WaitOne();
                Thread.Sleep(500);
            }
        }
        private void ConsumeProduct()
        {
            while (true)
            {
                Console.WriteLine("Now consume a product now");
                producer.Set();
                consumer.WaitOne();
                Thread.Sleep(500);
            }
        }
    }
}
