using ByLearning.SagaTransitionConfiguration;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;
using System;
using System.Threading.Tasks;

namespace ByLearning.SagaOrchestrator
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.Title = "Saga Orchestrator";
            var config = new ConfigurationOptions()
            {
                EndPoints = {
                      { GlobalConfiguration.GlobalSettings.RedisServerConfiguration.Server_Address,
                        GlobalConfiguration.GlobalSettings.RedisServerConfiguration.Server_Port }}
            };
            var connection =  ConnectionMultiplexer.Connect(config);
            IHost host = Host.CreateDefaultBuilder(args)
                    .ConfigureServices(services =>
                    {
                        services.AddSingleton(connection);
                        //工作线程，用于模拟发送消息
                        services.AddHostedService<OrchestratorService>();
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
                                context.ConfigureEndpoints(cfg);
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
