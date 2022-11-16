using ByLearning.PaymentServices.Models;
using ByLearning.SagaTransitionConfiguration;
using MassTransit;
using MassTransit.Courier.Messages;
using MassTransit.Metadata;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ByLearning.PaymentServices.Activity
{
    public class PaymentActivity : IExecuteActivity<IPaymentArgs>
    {
        private static string broker_address = GlobalConfiguration.GlobalSettings.RabbitMqConfiguration.Broker_Address;
        private static ushort broker_port = GlobalConfiguration.GlobalSettings.RabbitMqConfiguration.Broker_Port;
        private Uri stockCompensateAddress = new Uri($"rabbitmq://{broker_address}:{broker_port}/compensate_stock");
        public async Task<ExecutionResult> Execute(ExecuteContext<IPaymentArgs> context)
        {
            if (!context.Message.ActivityLogs.Select(o => o.Name).Contains("Payment"))
            {
                var paymentID = NewId.NextGuid();
                var host = new BusHostInfo(true);
                context.Message.ActivityLogs.Add(new RoutingSlipActivityLog(host, paymentID, "Payment", DateTime.Now, TimeSpan.Zero));
                context.Message.CompensateLogs.Add(new RoutingSlipCompensateLog(paymentID, this.stockCompensateAddress, null));
            }
            if (Faker.RandomNumber.Next(0,100) < 90)
            {
                Console.WriteLine($"{context.Arguments.CustomerName} Payment Successed!");
                return await Task.FromResult(context.Completed());
            }
            else
            {
                Console.WriteLine($"{context.Arguments.CustomerName} Payment Failed!");
                return await Task.FromResult(context.Faulted(new PaymentErrorException(Faker.Country.Name())));
            }
        }
    }
}
