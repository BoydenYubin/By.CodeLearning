using ByLearning.MassTransit.DemoRecord;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading.Tasks;
using IHost = Microsoft.Extensions.Hosting.IHost;
using Microsoft.Extensions.Logging;

namespace ByLearning.MassTransit
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.Title = "Masstransit Event Bus";
            IHost host = Host.CreateDefaultBuilder(args)
                             .ConfigureServices(services =>
                             {
                                 //工作线程，用于模拟发送消息
                                 services.AddHostedService<Worker>();
                                 //AddMassTransit 用于添加Masstransit相关依赖项
                                 services.AddMassTransit(configurator =>
                                 {
                                     //注册消费者
                                     configurator.AddConsumer<OrderCreatedEventConsumer>();
                                     //使用基于内存的消息路由传输
                                     configurator.UsingInMemory((context, cfg) =>
                                     {
                                         //主要用于配置接受节点ReceiveEndpoint
                                         cfg.ConfigureEndpoints(context);
                                     });
                                 });
                             })
                             
                             .ConfigureLogging(builder =>
                             {
                                 builder.AddConsole(options =>
                                 {
                                     options.LogToStandardErrorThreshold = LogLevel.Warning;
                                     options.TimestampFormat = "hh:mm:ss-fff";
                                 });
                             })
                           .Build();

            await host.RunAsync();
            Console.ReadLine();
        }
    }
}
