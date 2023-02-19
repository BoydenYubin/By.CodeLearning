using ByLearning.MassTransit.DemoRecord;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading.Tasks;
using IHost = Microsoft.Extensions.Hosting.IHost;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Collections.Generic;
using SkyApm.Diagnostics.MassTransit;
using SkyApm.AspNetCore.Diagnostics;

namespace ByLearning.MassTransit
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.Title = "Masstransit Event Bus";

            Activity.DefaultIdFormat = ActivityIdFormat.W3C;
            Activity.ForceDefaultIdFormat = true;
            Console.WriteLine("         {0,-15} {1,-60} {2,-15}", "OperationName", "Id", "Duration");
            ActivitySource.AddActivityListener(new ActivityListener()
            {
                ShouldListenTo = (source) => source.Name == "MassTransit",
                Sample = (ref ActivityCreationOptions<ActivityContext> options) => ActivitySamplingResult.AllDataAndRecorded,
                ActivityStarted = activity => Console.WriteLine("Started: {0,-15} {1,-60},{2, -70}", activity.OperationName, activity.Id, activity.ParentId),
                ActivityStopped = activity => Console.WriteLine("Stopped: {0,-15} {1,-60} {2,-15}", activity.OperationName, activity.Id, activity.Duration)
            });

            //var subscription = DiagnosticListener.AllListeners.Subscribe(new DiagnosticObserver());

            IHost host = Host.CreateDefaultBuilder(args)
                             .ConfigureServices(services =>
                             {
                                 //for skywalking
                                 services.AddSkyAPM(ext =>
                                 {
                                     ext.AddMasstransit();
                                     ext.AddAspNetCoreHosting();
                                 });
                                 //工作线程，用于模拟发送消息
                                 services.AddHostedService<Worker>();
                                 //AddMassTransit 用于添加Masstransit相关依赖项
                                 services.AddMassTransit(configurator =>
                                 {
                                     //注册消费者
                                     configurator.AddConsumer<OrderCreatedEventConsumer>();
                                     configurator.UsingRabbitMq((context, cfg) =>
                                     {
                                         cfg.Host("10.0.3.60", 5672, "/", config =>
                                            {
                                                config.Username("cap");
                                                config.Password("cap");
                                            });
                                         cfg.ConfigureEndpoints(context);
                                     });
                                     //使用基于内存的消息路由传输
                                     //configurator.UsingInMemory((context, cfg) =>
                                     //{
                                     //    //主要用于配置接受节点ReceiveEndpoint
                                     //    cfg.ConfigureEndpoints(context);
                                     //});
                                 });
                             })

                             .ConfigureLogging(builder =>
                             {
                                 //builder.AddConsole(options =>
                                 //{
                                 //    options.LogToStandardErrorThreshold = LogLevel.Warning;
                                 //    options.TimestampFormat = "hh:mm:ss-fff";
                                 //});
                             })
                           .Build();

            await host.RunAsync();
            Console.ReadLine();
        }
    }

    public class DiagnosticObserver : IObserver<DiagnosticListener>
    {
        public void OnCompleted() { }

        public void OnError(Exception error) { }

        public void OnNext(DiagnosticListener value)
        {
            if (value.Name == "MassTransit")
            {
                // subscribe to the listener with your monitoring tool, etc.
                value.Subscribe(new TestOutputListenerObserver());
            }
        }
    }


    public class TestOutputListenerObserver :
        IObserver<KeyValuePair<string, object>>
    {
        public void OnCompleted()
        {
        }

        public void OnError(Exception error)
        {
        }

        public void OnNext(KeyValuePair<string, object> value)
        {
            if (value.Key.EndsWith(".Start", StringComparison.OrdinalIgnoreCase))
            {
                var activity = Activity.Current;

                Console.WriteLine($"{DateTime.Now:HH:mm:ss.fff}-A Start {activity.OperationName} {activity.Id} {activity.ParentId}");
            }

            if (value.Key.EndsWith(".Stop", StringComparison.OrdinalIgnoreCase))
            {
                var activity = Activity.Current;

                Console.WriteLine(
                    $"{DateTime.Now:HH:mm:ss.fff}-A Stop  {activity.OperationName} {activity.Id} {activity.ParentId ?? "--"} {activity.Duration}");
            }
        }
    }
}
