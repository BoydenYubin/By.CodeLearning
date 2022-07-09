using System;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace ByLearningThread
{
    public class ChannelTest : TestWork
    {
        public void FirstSample()
        {
            //Channel是线程安全的
            var channel = Channel.CreateUnbounded<int>();
            System.Threading.CancellationTokenSource token = new System.Threading.CancellationTokenSource();
            Task.Run(async () =>
            {
                while (!token.IsCancellationRequested)
                {
                    var data = new Random().Next(200);
                    Console.WriteLine($"Writer Thread:{Thread.CurrentThread.ManagedThreadId}, Send: {data}");
                    await channel.Writer.WriteAsync(data);
                    token.Token.WaitHandle.WaitOne(new Random().Next(500, 1000));
                }
            },token.Token);
            Console.WriteLine("---------------------------");
            Task.Run(async () =>
            {
                while (!token.IsCancellationRequested)
                {
                    var res = await channel.Reader.ReadAsync();
                    Console.WriteLine($"Reader Thread:{Thread.CurrentThread.ManagedThreadId}, Receive:{res}");
                    token.Token.WaitHandle.WaitOne(new Random().Next(500, 1000));
                }
            },token.Token);
            Thread.Sleep(5000);
            token.Cancel();
        }
        
        public void SecondSample()
        {
            //Channel是线程安全的
            var channel = Channel.CreateUnbounded<int>();
            System.Threading.CancellationTokenSource token = new System.Threading.CancellationTokenSource();
            //创造两个生产者
            Task.Run(async () =>
            {
                while (!token.IsCancellationRequested)
                {
                    var data = new Random().Next(200);
                    Console.WriteLine($"WriterA Thread:{Thread.CurrentThread.ManagedThreadId}, Send: {data}");
                    await channel.Writer.WriteAsync(data);
                    token.Token.WaitHandle.WaitOne(new Random().Next(500, 1000));
                }
            }, token.Token);
            Task.Run(async () =>
            {
                while (!token.IsCancellationRequested)
                {
                    var data = new Random().Next(200);
                    Console.WriteLine($"WriterB Thread:{Thread.CurrentThread.ManagedThreadId}, Send: {data}");
                    await channel.Writer.WriteAsync(data);
                    token.Token.WaitHandle.WaitOne(new Random().Next(500, 1000));
                }
            }, token.Token);
            //创造一个消费者
            Task.Run(async () =>
            {
                while (!token.IsCancellationRequested)
                {
                    var res = await channel.Reader.ReadAsync();
                    Console.WriteLine($"Reader Thread:{Thread.CurrentThread.ManagedThreadId}, Receive:{res}");
                    token.Token.WaitHandle.WaitOne(new Random().Next(200, 400));
                }
            }, token.Token);
            Thread.Sleep(5000);
            token.Cancel();
        }
    }
}
