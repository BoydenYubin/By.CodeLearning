using System;
using System.Threading;
using System.Threading.Tasks;

namespace ByLearningThread
{
    public class TaskTest : ITestWork
    {
        public void Run()
        {
            CancellationTokenSource cs = new CancellationTokenSource();
            var task1 = new Task(async token =>
            {
                while (!((CancellationToken)token).IsCancellationRequested)
                {
                    Console.WriteLine("task1");
                    Thread.Sleep(100);
                }
                await Task.CompletedTask;
            }, cs.Token);
            var task2 = new Task(async token =>
            {
                while (!((CancellationToken)token).IsCancellationRequested)
                {
                    Console.WriteLine("task2");
                    Thread.Sleep(100);
                }
                await Task.CompletedTask;
            }, cs.Token);
            task1.Start();
            task2.Start();
            var task = Task.WhenAll(task1, task2);  //none block
            Thread.Sleep(1000);
            cs.Cancel();
            Task.WaitAll(task); //block
            Console.WriteLine("Task all completed!");
        }
    }
}
