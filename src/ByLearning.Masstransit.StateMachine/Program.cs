using ByLearning.Masstransit.StateMachine.StateMachineModel;
using ByLearning.Masstransit.StateMachine.Worker;
using ByLearning.SagaTransitionConfiguration;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using IHost = Microsoft.Extensions.Hosting.IHost;

namespace ByLearning.Masstransit.StateMachine
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.Title = "Masstransit StateMachine";
            Activity.DefaultIdFormat = ActivityIdFormat.W3C;
            Activity.ForceDefaultIdFormat = true;
            Console.WriteLine("         {0,-15} {1,-60} {2,-15}", "OperationName", "Id", "Duration");
            ActivitySource.AddActivityListener(new ActivityListener()
            {
                ShouldListenTo = (source) => source.Name == "MassTransit",
                Sample = (ref ActivityCreationOptions<ActivityContext> options) => ActivitySamplingResult.AllDataAndRecorded,
                ActivityStarted = activity =>
                { 
                    Console.WriteLine("Started: {0,-15} {1,-60},{2, -70}", activity.OperationName, activity.Id, activity.ParentId);
                },
                ActivityStopped = activity =>
                {
                    Console.WriteLine("Stopped: {0,-15} {1,-60} {2,-15}", activity.OperationName, activity.Id, activity.Duration);
                    foreach (var tags in activity.Tags)
                    {
                        if (tags.Key.Contains("messaging.masstransit.saga_id"))
                        {
                            Console.WriteLine($"Activity ID: {activity.Id}, Saga_ID : {tags.Value}");
                        }
                        if (tags.Key.Contains("messaging.masstransit.begin_state"))
                        {
                            Console.WriteLine($"Activity ID: {activity.Id}, Begin_state : {tags.Value}");
                        }
                        if (tags.Key.Contains("messaging.masstransit.tracking_number"))
                        {
                            Console.WriteLine($"Activity ID: {activity.Id}, Tracking_number : {tags.Value}");
                        }
                    }
                }
            });
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
                                     .RedisRepository(config =>
                                     {
                                         config.DatabaseConfiguration(GlobalConfiguration.GlobalSettings.RedisServerConfiguration.Server_Address);
                                     });
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
