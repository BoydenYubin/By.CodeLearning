using ByLearning.SagaTransitionConfiguration;
using MassTransit;
using MassTransit.Courier.Contracts;
using MassTransit.Metadata;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ByLearning.SagaOrchestrator
{
    public class OrchestratorService : BackgroundService
    {
        private readonly IBusControl bus;
        private readonly ILogger<OrchestratorService> logger;
        private readonly IDatabase database;
        private static string broker_address = GlobalConfiguration.GlobalSettings.RabbitMqConfiguration.Broker_Address;
        private static ushort broker_port = GlobalConfiguration.GlobalSettings.RabbitMqConfiguration.Broker_Port;
        private Uri orderAddress = new Uri($"rabbitmq://{broker_address}:{broker_port}/execute_order");
        private Uri orderCompensateAddress = new Uri($"rabbitmq://{broker_address}:{broker_port}/compensate_order");
        private Uri stockAddress = new Uri($"rabbitmq://{broker_address}:{broker_port}/execute_stock");
        private Uri stockCompensateAddress = new Uri($"rabbitmq://{broker_address}:{broker_port}/compensate_stock");
        private Uri paymentAddress = new Uri($"rabbitmq://{broker_address}:{broker_port}/execute_payment");

        public OrchestratorService(IBusControl bus,
            ConnectionMultiplexer connection,
            ILogger<OrchestratorService> logger)
        {
            this.bus = bus;
            this.logger = logger;
            this.database = connection.GetDatabase();
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (true)
            {
                await Task.Delay(TimeSpan.FromMilliseconds(50));

                try
                {
                    var customerName = (await database.ListRightPopAsync("order_trans")).ToString();
                    if (string.IsNullOrEmpty(customerName))
                        continue;
                    var orderInfo = new Dictionary<string, int>();
                    foreach (var entry in await database.HashGetAllAsync(customerName))
                    {
                        orderInfo.Add(entry.Name, ((int)entry.Value));
                    }
                    #region build activity
                    var trackingNumber = NewId.NextGuid();
                    var builder = new RoutingSlipBuilder(trackingNumber);

                    //var host = new BusHostInfo(true);
                    //var orderID = NewId.NextGuid();
                    builder.AddActivity("Order", orderAddress);

                    //var stockID = NewId.NextGuid();
                    builder.AddActivity("Stock", stockAddress);
                    //builder.AddActivityLog(host, "Stock", stockID, DateTime.Now, TimeSpan.Zero);
                    //builder.AddCompensateLog(stockID, orderCompensateAddress, null);

                    //var paymentID = NewId.NextGuid();
                    builder.AddActivity("Payment", paymentAddress);
                    //builder.AddActivityLog(host, "Payment", paymentID, DateTime.Now, TimeSpan.Zero);
                    //builder.AddCompensateLog(paymentID, stockCompensateAddress, null);
                    #endregion

                    builder.SetVariables(new
                    {
                        CustomerName = customerName,
                        RequestID = trackingNumber,
                        OrderInfo = orderInfo,
                    });
                    RoutingSlip routingSlip = builder.Build();
                    logger.LogInformation($"New Order from {customerName} at {DateTime.Now}");
                    await bus.Execute(routingSlip);
                }
                catch
                {
                    continue;
                }
                //await bus.Publish<IRoutingSlipCreated>(new
                //{
                //    TrackingNumber = routingSlip.TrackingNumber,
                //    Timestamp = routingSlip.CreateTimestamp,
                //});
            }
        }
    }
}
