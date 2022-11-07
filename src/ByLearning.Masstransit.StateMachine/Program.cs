using ByLearning.Masstransit.StateMachine.StateMachineModel;
using ByLearning.Masstransit.StateMachine.Worker;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading.Tasks;
using IHost = Microsoft.Extensions.Hosting.IHost;

namespace ByLearning.Masstransit.StateMachine
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.Title = "Masstransit StateMachine";
            IHost host = Host.CreateDefaultBuilder(args)
                             .ConfigureServices(services =>
                             {
                                 //工作线程，用于模拟发送消息
                                 services.AddHostedService<WorkerServices>();
                                 //AddMassTransit 用于添加Masstransit相关依赖项
                                 services.AddMassTransit(configurator =>
                                 {
                                     ///<seealso cref="MassTransit.Configuration.BusRegistrationContext"/>
                                     configurator.AddSagaStateMachine<ByLearningStateMachine, ByLearningState>()
                                     .InMemoryRepository();
                                     configurator.UsingInMemory((cfg, context) =>
                                     {
                                         //主要用于配置接受节点ReceiveEndpoint
                                         cfg.ConfigureEndpoints(context, default);
                                     });
                                 });
                             })
                           .Build();
            await host.RunAsync();
            Console.ReadLine();
        }
    }
}
