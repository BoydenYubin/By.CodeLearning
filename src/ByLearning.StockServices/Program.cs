using ByLearning.SagaTransitionConfiguration;
using ByLearning.StockServices.Activity;
using ByLearning.StockServices.Models;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SkyApm.AspNetCore.Diagnostics;
using SkyApm.Diagnostics.MassTransit;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace ShoppingCartServices
{
    class Program
    {
        static async Task Main(string[] args)
        {
            //config the ActivityListener 
            Activity.DefaultIdFormat = ActivityIdFormat.W3C;
            Activity.ForceDefaultIdFormat = true;
            ActivitySource.AddActivityListener(new ActivityListener()
            {
                ShouldListenTo = (source) => source.Name == "MassTransit",
                Sample = (ref ActivityCreationOptions<ActivityContext> options) => ActivitySamplingResult.AllDataAndRecorded,
                ActivityStarted = activity => { },
                ActivityStopped = activity => { }
            });
            Console.Title = "Stock Service";
            var reduceStockQueueName = "execute_stock";
            var compensateStockQueueName = "compensate_stock";
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
                                 //services.AddHostedService<CartHostedService>();
                                 //AddMassTransit 用于添加Masstransit相关依赖项
                                 services.AddMassTransit(configurator =>
                                 {
                                     configurator.UsingRabbitMq((cfg, context) =>
                                     {
                                         context.Host(host: GlobalConfiguration.GlobalSettings.RabbitMqConfiguration.Broker_Address,
                                                      port: GlobalConfiguration.GlobalSettings.RabbitMqConfiguration.Broker_Port,
                                                      virtualHost: GlobalConfiguration.GlobalSettings.RabbitMqConfiguration.VirtualHost,
                                                      cfg =>
                                                      {
                                                          cfg.Username(GlobalConfiguration.GlobalSettings.RabbitMqConfiguration.Username);
                                                          cfg.Password(GlobalConfiguration.GlobalSettings.RabbitMqConfiguration.Password);
                                                      });
                                         //主要用于配置接受节点ReceiveEndpoint
                                         context.ReceiveEndpoint(reduceStockQueueName, cfg =>
                                         {
                                             cfg.PrefetchCount = GlobalConfiguration.GlobalSettings.PrefetchCount;
                                             cfg.ConcurrentMessageLimit = GlobalConfiguration.GlobalSettings.ConcurrentMessageLimit;
                                             cfg.ExecuteActivityHost<StockActivity, IStockArgs>(c => c.UseRetry(r => r.Immediate(5)));
                                         });
                                         context.ReceiveEndpoint(compensateStockQueueName, e =>
                                         {
                                             e.PrefetchCount = GlobalConfiguration.GlobalSettings.PrefetchCount;
                                             e.ConcurrentMessageLimit = GlobalConfiguration.GlobalSettings.ConcurrentMessageLimit;
                                             e.CompensateActivityHost<StockActivity, IStockLogs>(c => c.UseRetry(r => r.Interval(3, TimeSpan.FromSeconds(2))));
                                         });
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
