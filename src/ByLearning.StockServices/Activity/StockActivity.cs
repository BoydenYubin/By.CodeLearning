using ByLearning.SagaTransitionConfiguration;
using ByLearning.StockServices.Models;
using MassTransit;
using MassTransit.Courier.Messages;
using MassTransit.Metadata;
using StackExchange.Redis;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ByLearning.StockServices.Activity
{
    public class StockActivity : IActivity<IStockArgs,IStockLogs>
    {
        private static string broker_address = GlobalConfiguration.GlobalSettings.RabbitMqConfiguration.Broker_Address;
        private static ushort broker_port = GlobalConfiguration.GlobalSettings.RabbitMqConfiguration.Broker_Port;
        private Uri orderCompensateAddress = new Uri($"rabbitmq://{broker_address}:{broker_port}/compensate_order");

        public async Task<CompensationResult> Compensate(CompensateContext<IStockLogs> context)
        {
            //redis事务执行
            var database = GetConnectedDatabase();
            #region lua script to reduce the stock
            /// 下一个事务失败则回滚stock数量
            /// 补偿时注意，如果补偿原来数量必须少于库存
            var script = @"for i, v in pairs(KEYS) do
                             redis.call('incrby',KEYS[i],ARGV[i])
                             redis.call('decrby', 'consumed_'..KEYS[i], ARGV[i])
                           end
                           return true";
            #endregion
            var keys = context.Log.OrderInfo.Select(o => new RedisKey(o.Key)).ToArray();
            var args = context.Log.OrderInfo.Select(o => {
                var tmp = new RedisValue();
                tmp = o.Value; return tmp;
            }).ToArray();
            var result = await database.ScriptEvaluateAsync(script, keys, args);
            System.Console.WriteLine("Compensated");
            if (((bool)result))
                return context.Compensated();
            else
                return context.Failed();
        }

        public async Task<ExecutionResult> Execute(ExecuteContext<IStockArgs> context)
        {
            if(!context.Message.ActivityLogs.Select(o => o.Name).Contains("Stock"))
            {
                var stockID = NewId.NextGuid();
                var host = new BusHostInfo(true);
                context.Message.ActivityLogs.Add(new RoutingSlipActivityLog(host, stockID, "Stock", DateTime.Now, TimeSpan.Zero));
                context.Message.CompensateLogs.Add(new RoutingSlipCompensateLog(stockID, this.orderCompensateAddress, null));
            }
            //redis事务执行
            var database = GetConnectedDatabase();

            #region lua script to reduce the stock
            /// 任何一个订单扣减失败，则会回滚stock数量
            var script = @"local results = {}
            
                           local function check(decrResults)
                             local res = true
                             for i, result in pairs(decrResults) do
                               res = res and (result >= 0)
                             end
                             return res
                           end
                           
                           for i, v in pairs(KEYS) do
                               results[i] = redis.call('decrby',KEYS[i], ARGV[i])
                               redis.call('incrby', 'consumed_'..KEYS[i], ARGV[i])
                           end
                           	
                           if(check(results)) then
                             return true;
                           else 
                             for i, v in pairs(KEYS) do
                               redis.call('incrby',KEYS[i],ARGV[i])
                               redis.call('decrby','consumed_'..KEYS[i], ARGV[i])
                             end
                             return false
                           end";
            #endregion

            var keys = context.Arguments.OrderInfo.Select(o => new RedisKey(o.Key)).ToArray();
            var args = context.Arguments.OrderInfo.Select(o => { 
                var tmp = new RedisValue(); 
                tmp = o.Value; return tmp;
                }).ToArray();
            var result = await database.ScriptEvaluateAsync(script, keys, args);
            if (((bool)result))
            {
                System.Console.WriteLine($"Stock Reduced {context.Arguments.CustomerName}");
                return context.Completed();
            }
            else
            {
                System.Console.WriteLine($"Stock Reduced Failed {context.Arguments.CustomerName}");
                foreach (var order in context.Arguments.OrderInfo)
                {
                    System.Console.WriteLine($"{order.Key} + {order.Value}");
                }
                System.Console.WriteLine("- - - - - - - - - - - - - - - - - - - ");
                return context.Faulted();
            }
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