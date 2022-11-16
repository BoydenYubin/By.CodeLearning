using ByLearning.SagaTransitionConfiguration;
using MassTransit;
using MassTransit.Courier.Messages;
using MassTransit.Metadata;
using OrderService.Models;
using StackExchange.Redis;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace OrderService.Activity
{
    public class OrderActivity : IActivity<IOrderInfoArgs, IOrderInfoLogs>
    {    
        public async Task<CompensationResult> Compensate(CompensateContext<IOrderInfoLogs> context)
        {
            //redis补偿事宜
            var database = GetConnectedDatabase();
            var transtion = database.CreateTransaction();
            var entrys = context.Log.OrderInfo.Select(o => new HashEntry(o.Key, o.Value)).ToArray();
            transtion.HashSetAsync(context.Log.CustomerName, entrys);
            await transtion.ExecuteAsync();
            System.Console.WriteLine($"Restore {context.Log.CustomerName}'s Order...");
            return await Task.FromResult(context.Compensated());
        }

        public async Task<ExecutionResult> Execute(ExecuteContext<IOrderInfoArgs> context)
        {
            //redis事务执行
            var database = GetConnectedDatabase();
            var transtion = database.CreateTransaction();

            transtion.KeyDeleteAsync(context.Arguments.CustomerName);
            await transtion.ExecuteAsync();
            return await Task.FromResult(context.Completed());
        }

        private IDatabase GetConnectedDatabase()
        {
            //redis事务执行
            var config = new ConfigurationOptions()
            {
                EndPoints = { { GlobalConfiguration.GlobalSettings.RedisServerConfiguration.Server_Address,
                                GlobalConfiguration.GlobalSettings.RedisServerConfiguration.Server_Port } }
            };
            return ConnectionMultiplexer.Connect(config).GetDatabase();
        }
    }
}
