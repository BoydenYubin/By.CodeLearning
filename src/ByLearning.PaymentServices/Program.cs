using ByLearning.PaymentServices.Activity;
using ByLearning.PaymentServices.Models;
using ByLearning.SagaTransitionConfiguration;
using MassTransit;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace ShoppingCartServices
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.Title = "Payment Service";
            var paymentQueueName = "execute_payment";
            IHost host = Host.CreateDefaultBuilder(args)
                   .ConfigureServices(services =>
                   {
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
                               context.ReceiveEndpoint(paymentQueueName, cfg =>
                               {
                                   cfg.PrefetchCount = GlobalConfiguration.GlobalSettings.PrefetchCount;
                                   cfg.ConcurrentMessageLimit = GlobalConfiguration.GlobalSettings.ConcurrentMessageLimit;
                                   //重试策略
                                   cfg.ExecuteActivityHost<PaymentActivity, IPaymentArgs>(c => c.UseRetry(r => r.Immediate(5)));
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
