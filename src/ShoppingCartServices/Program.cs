using ByLearning.SagaTransitionConfiguration;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using OrderService.Activity;
using OrderService.Models;
using SkyApm.AspNetCore.Diagnostics;
using SkyApm.Diagnostics.MassTransit;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace OrderService
{
    class Program
    {
        static async Task Main(string[] args)
        {
            //config the ActivityListener 
            System.Diagnostics.Activity.DefaultIdFormat = ActivityIdFormat.W3C;
            System.Diagnostics.Activity.ForceDefaultIdFormat = true;
            ActivitySource.AddActivityListener(new ActivityListener()
            {
                ShouldListenTo = (source) => source.Name == "MassTransit",
                Sample = (ref ActivityCreationOptions<ActivityContext> options) => ActivitySamplingResult.AllDataAndRecorded,
                ActivityStarted = activity => { },
                ActivityStopped = activity => { }
            });
            Console.Title = "Order Service";
            var orderQueueName = "execute_order";
            var orderCompensateQueueName = "compensate_order";
            IHost host = Host.CreateDefaultBuilder(args)
                    .ConfigureServices(services =>
                    {
                        //for skywalking
                        services.AddSkyAPM(ext =>
                        {
                            ext.AddMasstransit();
                            ext.AddAspNetCoreHosting();
                        });
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
                                context.ReceiveEndpoint(orderQueueName, cfg =>
                                {
                                    cfg.PrefetchCount = GlobalConfiguration.GlobalSettings.PrefetchCount;
                                    cfg.ConcurrentMessageLimit = GlobalConfiguration.GlobalSettings.ConcurrentMessageLimit;
                                    cfg.ExecuteActivityHost<OrderActivity, IOrderInfoArgs>(c => c.UseRetry(r => r.Immediate(5)));
                                });
                                context.ReceiveEndpoint(orderCompensateQueueName, e =>
                                {
                                    e.PrefetchCount = GlobalConfiguration.GlobalSettings.PrefetchCount;
                                    e.ConcurrentMessageLimit = GlobalConfiguration.GlobalSettings.ConcurrentMessageLimit;
                                    e.CompensateActivityHost<OrderActivity, IOrderInfoLogs>(c => c.UseRetry(r => r.Immediate(5)));
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
